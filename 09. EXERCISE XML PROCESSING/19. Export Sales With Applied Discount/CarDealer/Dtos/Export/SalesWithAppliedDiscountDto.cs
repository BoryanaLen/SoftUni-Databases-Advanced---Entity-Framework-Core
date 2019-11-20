using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("sale")]
    public class SalesWithAppliedDiscountDto
    {
        [XmlElement(ElementName = "car")]
        public CarsWithDistanceDto Car { get; set; }


        [XmlElement(ElementName = "discount")]
        public decimal Discount { get; set; }


        [XmlElement(ElementName = "customer-name")]
        public string CustomerName { get; set; }

        [XmlElement(ElementName = "price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName = "price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
    }

}
