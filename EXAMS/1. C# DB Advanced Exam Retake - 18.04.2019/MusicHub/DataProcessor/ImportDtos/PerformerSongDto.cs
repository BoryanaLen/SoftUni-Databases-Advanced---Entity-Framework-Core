using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Song")]
    public class PerformerSongDto
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }
    }
}
