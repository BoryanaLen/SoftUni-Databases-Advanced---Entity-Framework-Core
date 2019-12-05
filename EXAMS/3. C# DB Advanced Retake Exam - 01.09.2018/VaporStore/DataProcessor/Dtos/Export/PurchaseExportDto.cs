using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dtos.Export
{
    [XmlType("Purchase")]
    public class PurchaseExportDto
    {
        public string Card { get; set; }
        public string Cvc { get; set; }
        public string Date { get; set; }
        public GameExportDto Game { get; set; }
    }
}
