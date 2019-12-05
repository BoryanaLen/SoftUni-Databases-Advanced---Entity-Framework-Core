namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Count > 0))
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = string.Format($"{m.Rating:F2}"),
                    TotalIncomes = string.Format($"{m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)):F2}"),
                    Customers = m.Projections
                    .SelectMany(p => p.Tickets)
                    .Select(t => new
                    {
                        FirstName = t.Customer.FirstName,
                        LastName = t.Customer.LastName,
                        Balance = string.Format($"{t.Customer.Balance:F2}")
                    })
                    .OrderByDescending(c => c.Balance)
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName)
                    .ToArray()
                })
                .OrderByDescending(m => double.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToArray();

            var result = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers
                .Where(c => c.Age >= age)
                .Select(c => new CustomerExportDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = string.Format($"{c.Tickets.Sum(t => t.Price):F2}"),
                    SpentTime = TimeSpan.FromSeconds(
                            c.Tickets.Sum(s => s.Projection.Movie.Duration.TotalSeconds))
                        .ToString(@"hh\:mm\:ss")
                })               
                .OrderByDescending(c => decimal.Parse(c.SpentMoney))
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(CustomerExportDto[]), new XmlRootAttribute("Customers"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), customers, namespaces);

            string result = sb.ToString().TrimEnd();

            return result;
        }
    }
}