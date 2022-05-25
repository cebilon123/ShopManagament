using System;
using System.Collections.Generic;

namespace Api.Result
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public User Worker { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string ShipmentMethod { get; set; }
        public string PaymentMethod { get; set; }
        public bool OrderPaid { get; set; }
        public bool OrderSend { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderPaymentDate { get; set; }
        public DateTime OrderSendDate { get; set; }
        public List<OrderedProduct> OrderedProducts { get; set; }
    }
}