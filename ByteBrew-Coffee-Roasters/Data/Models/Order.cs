namespace ByteBrew_Coffee_Roasters.Data.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public IList<Product>? Products { get; set; }

        public Order()
        {
        }

        public Order(string status)
        {
            Id = Guid.NewGuid();
            Status = status;
            Products = new List<Product>();
        }
    }
}
