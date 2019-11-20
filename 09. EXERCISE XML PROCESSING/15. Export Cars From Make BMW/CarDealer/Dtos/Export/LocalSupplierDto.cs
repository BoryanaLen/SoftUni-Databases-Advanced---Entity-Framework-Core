using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("suplier")]
    public class LocalSupplierDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PartsCount { get; set; }
    }
}
