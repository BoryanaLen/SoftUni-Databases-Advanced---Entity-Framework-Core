using System.ComponentModel.DataAnnotations;

namespace PetClinic.DataProcessor.Dtos.Import
{
    public class AnimalImportDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Type { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public int Age { get; set; }
        public string PassportSerialNumber { get; set; }

        [Required]
        public PassportImportDto Passport { get; set; }

    }
}
