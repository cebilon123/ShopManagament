namespace Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Left { get; set; } // ile zostalo w magazynie
        public float Weight { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}