namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writersList = JsonConvert.DeserializeObject<List<WriterImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Writer> validWriters = new List<Writer>();

            foreach (var writerDto in writersList)
            {
                if (!IsValid(writerDto))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Writer writer = new Writer();
                writer.Name = writerDto.Name;
                writer.Pseudonym = writerDto.Pseudonym;

                validWriters.Add(writer);

                sb.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));
            }

            context.Writers.AddRange(validWriters);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var prodicersList = JsonConvert.DeserializeObject<List<ProducerImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Producer> validProducers = new List<Producer>();

            foreach (var producerDto in prodicersList)
            {
                if (!IsValid(producerDto))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                List<Album> validAlbums = new List<Album>();

                bool areValidAlbums = true;

                foreach (var albumDto in producerDto.Albums)
                {
                    if (!IsValid(albumDto))
                    {
                        sb.AppendLine(ErrorMessage);

                        areValidAlbums = false;

                        break;
                    }
                }

                if (!areValidAlbums)
                {
                    continue;
                }
                else
                {
                    foreach (var albumDto in producerDto.Albums)
                    {
                        var album = new Album()
                        {
                            Name = albumDto.Name,
                            ReleaseDate = DateTime.ParseExact(albumDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        };

                        validAlbums.Add(album);
                    }

                    var producer = new Producer()
                    {
                        Name = producerDto.Name,
                        Pseudonym = producerDto.Pseudonym,
                        PhoneNumber = producerDto.PhoneNumber,
                        Albums = validAlbums
                    };

                    validProducers.Add(producer);

                    if(producer.PhoneNumber == null)
                    {
                        sb.AppendLine(string.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name,
                            producer.Albums.Count));
                    }
                    else
                    {
                        sb.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone, producer.Name,
                           producer.PhoneNumber, producer.Albums.Count));
                    }

                    context.Albums.AddRange(validAlbums);

                    context.SaveChanges();
                }
            }

            context.Producers.AddRange(validProducers);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SongImportDto[]), new XmlRootAttribute("Songs"));

            var songsDto = (SongImportDto[])ser.Deserialize(new StringReader(xmlString));

            List<Song> validSongs = new List<Song>();

            StringBuilder sb = new StringBuilder();

            foreach (var songDto in songsDto)
            {
                var album = context.Albums.FirstOrDefault(a => a.Id == songDto.AlbumId);

                var writer = context.Writers.FirstOrDefault(w => w.Id == songDto.WriterId);

                if (!IsValid(songDto) || !Enum.IsDefined(typeof(Genre), songDto.Genre) 
                    || album == null || writer == null)
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Song song = new Song()
                {
                    Name = songDto.Name,
                    Duration = TimeSpan.ParseExact(songDto.Duration, "c", null),
                    CreatedOn = DateTime.ParseExact(songDto.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Genre = (Genre)Enum.Parse(typeof(Genre), songDto.Genre),
                    WriterId = songDto.WriterId,
                    AlbumId = songDto.AlbumId,
                    Price = songDto.Price
                };

                validSongs.Add(song);

                sb.AppendLine(string.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration));
            }

            context.Songs.AddRange(validSongs);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {

            XmlSerializer ser = new XmlSerializer(typeof(PerformerImportDto[]), new XmlRootAttribute("Performers"));

            var performersDto = (PerformerImportDto[])ser.Deserialize(new StringReader(xmlString));

            List<Performer> performers = new List<Performer>();

            StringBuilder sb = new StringBuilder();

            foreach (var performerDto in performersDto)
            {
                if (!IsValid(performerDto))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                List<SongPerformer> validSongsPerformers = new List<SongPerformer>();

                bool areValidSongs = true;

                foreach (var songDto in performerDto.PerformersSongs)
                {
                    if (!context.Songs.Any(s => s.Id == songDto.Id))
                    {
                        sb.AppendLine(ErrorMessage);

                        areValidSongs = false;

                        break;
                    }
                }

                if (!areValidSongs)
                {
                    continue;
                }
                else
                {
                    var performer = new Performer()
                    {
                        FirstName = performerDto.FirstName,
                        LastName = performerDto.LastName,
                        Age = performerDto.Age,
                        NetWorth = performerDto.NetWorth
                    };

                    foreach (var songDto in performerDto.PerformersSongs.Distinct())
                    {
                        var song = context.Songs.FirstOrDefault(s => s.Id == songDto.Id);

                        var songPerformer = new SongPerformer()
                        {
                            Song = song,
                            Performer = performer
                        };

                        validSongsPerformers.Add(songPerformer);
                    }

                    performer.PerformerSongs = validSongsPerformers;

                    context.SongsPerformers.AddRange(validSongsPerformers);

                    context.Performers.Add(performer);

                    sb.AppendLine(string.Format(SuccessfullyImportedPerformer, performer.FirstName, 
                        performer.PerformerSongs.Count));

                    context.SaveChanges();
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);

            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}