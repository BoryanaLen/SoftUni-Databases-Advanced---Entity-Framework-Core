namespace Petstore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataValidation;

    public class Food
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        public double Weight { get; set; } //In kg.
        public decimal DestributorPrice { get; set; }
        public decimal Price { get; set; }
        public DateTime EnspirationDate { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<FoodOrder> Orders { get; set; } = new HashSet<FoodOrder>();
    }
}
