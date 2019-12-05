using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dtos.Export
{
    [XmlType("Game")]
    public class GameExportDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }
        public string Genre { get; set; }

        public decimal Price { get; set; }
    }
}
