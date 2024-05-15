﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ByteBrew_Coffee_Roasters.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public int QuantityInCart { get; set; }
        public IList<Order>? Orders { get; set; }

        public Product()
        {
            Name = string.Empty;
        }

        public Product(string name, string? description, float price, string? image)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Image = image;
        }
    }
}
