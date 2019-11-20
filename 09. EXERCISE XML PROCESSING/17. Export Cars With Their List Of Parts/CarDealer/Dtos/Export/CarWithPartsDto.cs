using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("car")]
    public class CarWithPartsDto
    {
        [XmlAttribute(AttributeName = "make")]
        public string Make { get; set; }


        [XmlAttribute(AttributeName = "model")]
        public string Model { get; set; }


        [XmlAttribute(AttributeName = "travelled-distance")]
        public long TravelledDistance { get; set; }


        [XmlArray(ElementName = "parts")]
        public PartDto[] Parts { get; set; }

    }
}
