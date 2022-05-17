namespace Api.Commands
{
    public class CreateProductCommand
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int Left { get; set; }
        public float Weight { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}