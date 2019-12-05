using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dtos.Export
{
    [XmlType("AnimalAid")]
    public class AnimalAidExportDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

    }
}
