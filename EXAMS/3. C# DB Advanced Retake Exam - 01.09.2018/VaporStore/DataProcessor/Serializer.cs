namespace VaporStore.DataProcessor
{
	using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dtos.Export;
    using Formatting = Newtonsoft.Json.Formatting;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var games = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games.Where(ga => ga.Purchases.Count > 0)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Title = x.Name,
                        Developer = x.Developer.Name,
                        Tags = string.Join(", ", x.GameTags.Select(gt => gt.Tag.Name)),
                        Players = x.Purchases.Count
                    })
                    .OrderByDescending(x => x.Players)
                    .ThenBy(x => x.Id)
                    .ToArray(),
                    TotalPlayers = g.Games.Sum(ga => ga.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToArray();


            var result = JsonConvert.SerializeObject(games, Formatting.Indented);

            return result;
        }

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            var users = context.Users
                .Select(u => new UserExportDto
                {
                    Username = u.Username,
                    Purchases = u.Cards
                    .SelectMany(c => c.Purchases)
                    .Where(p => p.Type.ToString() == storeType)
                    .Select(p => new PurchaseExportDto
                    {
                        Card = p.Card.Number,
                        Cvc = p.Card.Cvc,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new GameExportDto
                        {
                            Title = p.Game.Name,
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price
                        }
                    })
                    .OrderBy(p => p.Date)
                    .ToArray(),
                    TotalSpent = u.Cards.SelectMany(c => c.Purchases)
                        .Where(p => p.Type.ToString() == storeType)
                        .Select(x => x.Game)
                        .Sum(y => y.Price)
                })
                .Where(u => u.Purchases.Count() > 0)
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();


            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(UserExportDto[]), new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), users, namespaces);

            string result = sb.ToString().TrimEnd();

            return result;
        }
	}
}