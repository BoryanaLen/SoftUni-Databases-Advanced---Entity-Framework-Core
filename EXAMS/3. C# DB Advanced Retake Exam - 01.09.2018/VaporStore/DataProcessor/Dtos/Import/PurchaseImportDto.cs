using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dtos.Import
{
    [XmlType("Purchase")]
    public class PurchaseImportDto
    {
        [Required]
        [XmlAttribute(AttributeName = "title")]
        public string Game { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [RegularExpression("[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}")]
        public string Key { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Card { get; set; }

    }
}
