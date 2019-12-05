namespace MusicHub.DataProcessor
{
    using Data;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        Price = string.Format($"{s.Price:F2}"),
                        Writer = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.Writer)
                    .ToArray(),
                    AlbumPrice = string.Format($"{a.Songs.Sum(s => s.Price):F2}")
                })
                .OrderByDescending(a => decimal.Parse(a.AlbumPrice))
                .ToArray();

            var result = JsonConvert.SerializeObject(albums, Formatting.Indented);

            return result;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new SongExportDto
                {
                   SongName = s.Name,
                   Writer = s.Writer.Name,
                   Performer = s.SongPerformers.Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName).FirstOrDefault(),
                   AlbumProducer = s.Album.Producer.Name,
                   Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.Performer)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(SongExportDto[]), new XmlRootAttribute("Songs"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), songs, namespaces);

            string result = sb.ToString().TrimEnd();

            return result;
        }
    }
}