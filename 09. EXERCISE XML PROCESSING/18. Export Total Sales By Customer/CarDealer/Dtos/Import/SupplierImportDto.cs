using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Supplier")]
    public class SupplierImportDto
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }


        [XmlElement(ElementName = "isImporter")]
        public bool IsImporter { get; set; }
    }
}
