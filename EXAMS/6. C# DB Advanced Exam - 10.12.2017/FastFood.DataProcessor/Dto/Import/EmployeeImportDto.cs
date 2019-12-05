using System.ComponentModel.DataAnnotations;

namespace FastFood.DataProcessor.Dto.Import
{
    public class EmployeeImportDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(15, 80)]
        public int Age { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
