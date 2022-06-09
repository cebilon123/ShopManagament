using System.Collections.Generic;

namespace Api.Commands
{
    public class CreateOrderCommand
    {
        public int UserId { get; set; }
        public int WorkerId { get; set; }
        public string ShipmentMethod { get; set; }
        public string PaymentMethod { get; set; }
    }
}