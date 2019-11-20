using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlRoot(ElementName = "parts")]
    public class PartIdImportDto
    {
        [XmlAttribute(AttributeName = "id")]
        public int PartId { get; set; }
    }
}
