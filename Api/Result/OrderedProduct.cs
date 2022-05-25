namespace Api.Result
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
        
        public static OrderedProduct FromDatabase(Models.OrderedProduct model)
        {
            return new OrderedProduct
            {
                Category = model.Category,
                Count = model.Count,
                Description = model.Description,
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Weight = model.Weight,
                OrderActive = model.OrderActive
            };
        }
    }
}