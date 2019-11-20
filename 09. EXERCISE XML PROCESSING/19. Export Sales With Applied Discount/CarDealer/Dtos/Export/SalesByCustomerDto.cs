using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("customer")]
    public class SalesByCustomerDto
    {
        [XmlAttribute(AttributeName = "full-name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute(AttributeName = "spent-money")]
        public decimal SpentMoney { get; set; }
    }
}
