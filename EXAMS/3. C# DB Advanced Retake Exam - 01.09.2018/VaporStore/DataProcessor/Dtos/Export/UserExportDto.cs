using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dtos.Export
{
    [XmlType("User")]
    public class UserExportDto
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        public PurchaseExportDto[] Purchases { get; set; }

        public decimal TotalSpent { get; set; }

    }
}
