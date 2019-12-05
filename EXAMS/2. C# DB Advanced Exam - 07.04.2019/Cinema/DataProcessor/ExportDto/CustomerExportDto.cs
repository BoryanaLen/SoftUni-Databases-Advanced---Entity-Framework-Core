using System;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class CustomerExportDto
    {

        [XmlAttribute(AttributeName = "FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute(AttributeName = "LastName")]
        public string LastName { get; set; }

        public string SpentMoney { get; set; }

        public string SpentTime { get; set; }

    }
}
