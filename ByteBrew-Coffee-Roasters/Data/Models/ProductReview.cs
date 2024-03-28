using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteBrew_Coffee_Roasters.Data.Models
{
    public class ProductReview
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Product? Product { get; set; }
        public string Text { get; set; }

        public ProductReview(Guid productId, string text)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Text = text;
        }
    }
}
