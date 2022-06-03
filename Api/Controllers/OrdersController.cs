using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Commands;
using Api.Result;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }
        
        [HttpPost]
        public ActionResult<int> CreateOrder([FromBody]CreateOrderCommand command)
        {
            var result = _orderService.CreateOrder(command);
            return Created($"Orders/{result}", result);
        }

        [HttpPut("{orderId:int}/MarkAsPaid")]
        public void MarkAsPaid(int orderId, [FromBody]string paymentMethod)
        {
            _orderService.MarkAsPaid(orderId, paymentMethod);
        }
        
        [HttpPut("{orderId:int}/MarkAsSend")]
        public void MarkAsSend(int orderId)
        {
            _orderService.MarkAsSend(orderId);
        }
        
        [HttpPut("{orderId:int}/Archive")]
        public void ArchiveOrder(int orderId)
        {
            _orderService.Archive(orderId);
        }

        [HttpGet]
        public List<Order> GetOrders()
        {
            return _orderService.GetOrders();
        }
    }
}