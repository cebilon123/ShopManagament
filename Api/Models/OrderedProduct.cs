namespace Api.Models
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Count { get; set; }
        public float Weight { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool OrderActive { get; set; }

        // tak w entity framework robi sie foreign key dla innej tabeli
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}