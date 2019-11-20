using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Car")]
    public class CarImportDto
    {
        [XmlElement(ElementName = "make")]
        public string Make { get; set; }


        [XmlElement(ElementName = "model")]
        public string Model { get; set; }


        [XmlElement(ElementName = "TravelledDistance")]
        public long TravelledDistance { get; set; }

        [XmlElement(ElementName = "parts")]
        public ImportPartsDto Parts { get; set; }
    }
}
