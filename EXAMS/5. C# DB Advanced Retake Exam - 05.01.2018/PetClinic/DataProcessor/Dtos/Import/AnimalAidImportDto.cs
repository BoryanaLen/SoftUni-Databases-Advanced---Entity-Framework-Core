using System.ComponentModel.DataAnnotations;

namespace PetClinic.DataProcessor.Dtos.Import
{
    public class AnimalAidImportDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

    }
}
