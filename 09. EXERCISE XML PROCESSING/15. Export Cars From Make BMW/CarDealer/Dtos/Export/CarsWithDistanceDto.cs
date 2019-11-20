using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("car")]
    public class CarsWithDistanceDto
    {
        [XmlElement(ElementName = "make")]
        public string Make { get; set; }


        [XmlElement(ElementName = "model")]
        public string Model { get; set; }


        [XmlElement(ElementName = "travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}
