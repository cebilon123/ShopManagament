using System.Collections.Generic;

namespace Api.Commands
{
    public class CreateOrderCommand
    {
        public int UserId { get; set; }
        public int WorkerId { get; set; }
        public List<CreateOrderCommandProduct> Products { get; set; }
        public string ShipmentMethod { get; set; }
        public string PaymentMethod { get; set; }

        public class CreateOrderCommandProduct
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }
    }
}