using System;
using System.Collections.Generic;
using System.Text;
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
