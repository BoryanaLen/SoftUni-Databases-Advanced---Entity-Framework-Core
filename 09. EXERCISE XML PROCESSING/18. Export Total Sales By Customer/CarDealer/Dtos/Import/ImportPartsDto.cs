using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlRoot(ElementName = "parts")]
    public class ImportPartsDto
    {
        [XmlElement(ElementName = "partId")]
        public PartIdImportDto[] PartsId { get; set; }
    }
}
