using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class PrisonerExportDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IncarcerationDate { get; set; }
        public MessageExportDto[] EncryptedMessages { get; set; }

    }
}
