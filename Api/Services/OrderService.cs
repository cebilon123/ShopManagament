using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Commands;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using User = Api.Result.User;

namespace Api.Services
{
    public class OrderService
    {
        private readonly DatabaseContext _context;

        public OrderService(DatabaseContext context)
        {
            _context = context;
        }

        public int CreateOrder(CreateOrderCommand command)
        {
            // zaczynamy transakcje
            _context.Database.BeginTransaction();

            var order = new Order
            {
                UserId = command.UserId,
                WorkerId = command.WorkerId,
                OrderDate = DateTime.UtcNow,
                PaymentMethod = command.PaymentMethod,
                ShipmentMethod = command.ShipmentMethod,
            };

            // dodajemy do bazy nowy order oraz zaktualizowane produkty o ilosc do orderow
            _context.Orders.Add(order);

            // zapisujemy zmiany (by dodalo id zamowieniu)
            _context.SaveChanges();
            _context.Database.CommitTransactionAsync();

            return order.Id;
        }

        public List<Result.Order> GetOrders(int page, int resultsPerPage)
        {
            var result = new List<Result.Order>();
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Worker)
                .AsNoTracking()
                .Skip(page * resultsPerPage)
                .Take(resultsPerPage)
                .ToList();

            var ordersIds = orders.Select(o => o.Id).ToList();
            
            var orderedProducts = _context.OrderedProducts
                .Include(op => op.Product)
                .Where(op => ordersIds.Contains(op.OrderId))
                .AsNoTracking()
                .ToList();

            foreach (var order in orders)
            {
                result.Add(new Result.Order
                {
                    Id = order.Id,
                    Price = order.Price,
                    User = User.FromDatabase(order.User),
                    Weight = order.Weight,
                    Worker = User.FromDatabase(order.Worker),
                    OrderDate = order.OrderDate,
                    OrderPaid = order.OrderPaid,
                    OrderSend = order.OrderSend,
                    OrderSendDate = order.OrderSendDate,
                    PaymentMethod = order.PaymentMethod,
                    ShipmentMethod = order.ShipmentMethod,
                    OrderPaymentDate = order.OrderPaymentDate,
                    OrderedProducts = orderedProducts.Where(op => op.OrderId == order.Id)
                        .ToList()
                        .Select(Result.OrderedProduct.FromDatabase)
                        .ToList()
                });
            }
            
            return result;
        }

        public void MarkAsPaid(int orderId, string paymentMethod)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order is null)
                return;

            order.PaymentMethod = paymentMethod;
            order.OrderPaymentDate = DateTime.UtcNow;

            _context.Orders.Update(order);

            _context.SaveChanges();
        }
        
        public void MarkAsSend(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order is null)
                return;

            order.OrderSend = true;
            order.OrderSendDate = DateTime.UtcNow;

            _context.Orders.Update(order);

            _context.SaveChanges();
        }

        public void Archive(int orderId)
        {
            _context.ArchiveOrder(orderId);
        }

        public bool AddProductToOrder(AddProductToOrderCommand command)
        {
            var order = _context.Orders.AsNoTracking()
                .FirstOrDefault(o => o.Id == command.OrderId);

            var product = _context.Products
                .FirstOrDefault(p => p.Id == command.ProductId);

            if (order is null || product is null || product.Left < command.Amount)
            {
                return false;
            }

            var orderedProduct = new OrderedProduct
            {
                Category = product.Category,
                Count = command.Amount,
                Description = product.Description,
                Name = product.Name,
                OrderId = order.Id,
                Price = product.Price,
                Weight = product.Weight,
                OrderActive = true,
                ProductId = product.Id
            };
            product.Left -= command.Amount;

            _context.OrderedProducts.Add(orderedProduct);
            _context.Products.Update(product);
            _context.SaveChanges();
            
            return true;
        }
    }
}