using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dtos.Import
{
    [XmlType("Procedure")]
    public class ProcedureImportDto
    {
        [Required]
        public string Animal { get; set; }

        [Required]
        public string Vet { get; set; }

        [Required]
        public string DateTime { get; set; }

        public ProcedureAnimalAidImportDto[] AnimalAids { get; set; }
    }
}
