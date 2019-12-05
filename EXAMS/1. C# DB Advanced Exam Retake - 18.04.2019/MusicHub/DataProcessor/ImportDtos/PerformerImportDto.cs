using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Performer")]
    public class PerformerImportDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [Range(18, 70)]
        public int Age { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal NetWorth { get; set; }

        public PerformerSongDto [] PerformersSongs { get; set; } 

    }
}
