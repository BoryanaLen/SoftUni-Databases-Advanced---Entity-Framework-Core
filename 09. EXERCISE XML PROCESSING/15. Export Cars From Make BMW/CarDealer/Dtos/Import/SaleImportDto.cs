using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Sale")]
    public class SaleImportDto
    {
        [XmlElement(ElementName = "carId")]
        public int CarId { get; set; }

        [XmlElement(ElementName = "customerId")]
        public int CustomerId { get; set; }


        [XmlElement(ElementName = "discount")]
        public decimal Discount { get; set; }
    }
}
