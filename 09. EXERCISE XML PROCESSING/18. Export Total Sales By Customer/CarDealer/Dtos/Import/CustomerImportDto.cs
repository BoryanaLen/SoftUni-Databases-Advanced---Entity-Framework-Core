using System;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Customer")]
    public class CustomerImportDto
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }


        [XmlAttribute(AttributeName = "birthDate")]
        public DateTime BirthDate { get; set; }


        [XmlAttribute(AttributeName = "isYoungDriver")]
        public bool IsYoungDriver { get; set; }

    }
}
