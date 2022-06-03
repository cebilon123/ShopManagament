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
            var productsIds = command.Products.Select(p => p.Id);
            var products = _context.Products.Where(p => productsIds.Any(id => id == p.Id))
                .ToList();

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

            var orderedProducts = new List<OrderedProduct>();

            // sprawdzamy czy kazdy produkt moze zostac dodany do zamowienia
            // min. sprawdzamy czy jego ilosc moze zostac dodana do zamowienia, jesli nie
            // to dla uproszczenia projektu usuwamy go po prostu z dodawania do bazy
            float price = 0;
            float weight = 0;
            foreach (var product in products)
            {
                var productFromCommand = command.Products.FirstOrDefault(p => p.Id == product.Id);

                if (productFromCommand is null || product.Left == 0 || productFromCommand.Amount > product.Left)
                {
                    products.Remove(product);
                    continue;
                }

                product.Left -= productFromCommand.Amount;
                price += product.Price * productFromCommand.Amount;
                weight += product.Weight * productFromCommand.Amount;

                // dodajemy zamówione produkty
                orderedProducts.Add(new OrderedProduct
                {
                    Category = product.Category,
                    Description = product.Description,
                    ProductId = product.Id,
                    Count = productFromCommand.Amount,
                    Weight = product.Weight,
                    Name = product.Name,
                    Price = product.Price,
                    OrderActive = true,
                });
            }

            order.Price = price;
            order.Weight = weight;

            // dodajemy do bazy nowy order oraz zaktualizowane produkty o ilosc do orderow
            _context.Orders.Add(order);
            _context.Products.UpdateRange(products);

            // zapisujemy zmiany (by dodalo id zamowieniu)
            _context.SaveChanges();

            // jeszcze aktualizujemy produkty w zamówieniu
            orderedProducts.ForEach(p => { p.OrderId = order.Id; });

            // dodajemy ordered procuts, zapisujemy wszystko i commitujemy transakcje
            _context.OrderedProducts.AddRangeAsync(orderedProducts);
            _context.SaveChangesAsync();
            _context.Database.CommitTransactionAsync();

            return order.Id;
        }

        public List<Result.Order> GetOrders()
        {
            var result = new List<Result.Order>();
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Worker)
                .AsNoTracking()
                .ToList();
            var orderedProducts = _context.OrderedProducts
                .Include(op => op.Product)
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
    }
}