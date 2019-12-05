using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Projection")]
    public class ProjectionImportDto
    {
        [Required]
        public int MovieId { get; set; }


        [Required]
        public int HallId { get; set; }

        [Required]
        public string DateTime { get; set; }

    }
}
