namespace ByteBrew_Coffee_Roasters.Data.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public CustomerType CustomerType { get; set; } = CustomerType.Anonymous;
        public Guid? UserId { get; set; }
        public string Status { get; set; } = "Не задан";
        public List<OrderItem> Products { get; set; } = new List<OrderItem>();

        public Order(string status)
        {
            Status = status;
        }

        public Order(string status, Guid userId)
        {
            CustomerType = CustomerType.Authorizated;
            UserId = userId;
            Status = status;
        }
    }

    public enum CustomerType
    {
        Anonymous, Authorizated
    }
}
