namespace Api.Result
{
    /// <summary>
    /// Model resultatowy zwracany z api
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Left { get; set; }
        public float Weight { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public static Product FromModel(Models.Product model)
        {
            return new Product
            {
                Category = model.Category,
                Description = model.Description,
                Id = model.Id,
                Left = model.Left,
                Name = model.Name,
                Price = model.Price,
                Weight = model.Weight
            };
        }
    }
}