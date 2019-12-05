using System.ComponentModel.DataAnnotations;

namespace FastFood.DataProcessor.Dto.Import
{
    public class ItemImportDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
