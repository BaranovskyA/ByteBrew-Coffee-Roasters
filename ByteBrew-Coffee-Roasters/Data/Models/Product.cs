namespace ByteBrew_Coffee_Roasters.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }

        public IList<Order>? Orders { get; set; }

        public Product(string name, string? description, decimal price, string? image)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Image = image;
        }
    }
}
