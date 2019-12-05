using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dtos.Import
{
    [XmlType("AnimalAid")]
    public class ProcedureAnimalAidImportDto
    {
        public string Name { get; set; }
    }
}
