namespace VaporStore.DataProcessor
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
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dtos.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesList = JsonConvert.DeserializeObject<List<GameImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var gameDto in gamesList)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Length == 0 || gameDto.Tags.Any(t => t == string.Empty))
                {
                    sb.AppendLine("Invalid Data");

                    continue;
                }

                Game game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };


                if (!context.Developers.Any(d => d.Name == gameDto.Developer))
                {
                    Developer developer = new Developer
                    {
                        Name = gameDto.Developer
                    };

                    context.Developers.Add(developer);

                    context.SaveChanges();
                }

                if (!context.Genres.Any(g => g.Name == gameDto.Genre))
                {
                    Genre genre = new Genre
                    {
                        Name = gameDto.Genre
                    };

                    context.Genres.Add(genre);

                    context.SaveChanges();
                }

                game.Developer = context.Developers.First(d => d.Name == gameDto.Developer);
                game.DeveloperId = context.Developers.First(d => d.Name == gameDto.Developer).Id;
                game.Genre = context.Genres.First(g => g.Name == gameDto.Genre);
                game.GenreId = context.Genres.First(g => g.Name == gameDto.Genre).Id;

                List<GameTag> gameTags = new List<GameTag>();

                foreach (var tagDto in gameDto.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(t => t.Name == tagDto);

                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Name = tagDto
                        };

                        context.Tags.Add(tag);
                    }

                    GameTag gameTag = new GameTag
                    {
                        Game = game,
                        GameId = game.Id,
                        Tag = tag,
                        TagId = tag.Id
                    };

                    context.GameTags.Add(gameTag);


                    gameTags.Add(gameTag);
                }

                game.GameTags = gameTags;

                context.Games.Add(game);

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersList = JsonConvert.DeserializeObject<List<UserImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var userDto in usersList)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine("Invalid Data");

                    continue;
                }

                bool areAllCardsValid = true;

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto) )
                    {
                        sb.AppendLine("Invalid Data");

                        areAllCardsValid = false;

                        break;
                    }
                }

                if (areAllCardsValid)
                {
                    User user = new User
                    {
                        Username = userDto.Username,
                        FullName = userDto.FullName,
                        Email = userDto.Email,
                        Age = userDto.Age
                    };

                    List<Card> cardsList = new List<Card>();

                    foreach (var card in userDto.Cards)
                    {
                        Card newCard = new Card
                        {
                            Number = card.Number,
                            Cvc = card.Cvc,
                            Type = card.Type,
                            User = user
                        };                       

                        user.Cards.Add(newCard);

                        cardsList.Add(newCard);
                    }

                    context.Users.Add(user);

                    sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");

                    context.Cards.AddRange(cardsList);
                }
                else
                {
                    continue;
                }
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(PurchaseImportDto[]), new XmlRootAttribute("Purchases"));

            var purchasesDto = (PurchaseImportDto[])ser.Deserialize(new StringReader(xmlString));

            List<Purchase> purchases = new List<Purchase>();

            StringBuilder sb = new StringBuilder();

            foreach (var purchaseDto in purchasesDto)
            {
                Game game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.Game);

                Card card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card);

                if (!IsValid(purchaseDto) || !Enum.IsDefined(typeof(PurchaseType), purchaseDto.Type)
                    || game == null || card == null)
                {
                    sb.AppendLine("Invalid Data");

                    continue;
                }

                Purchase purchase = new Purchase
                {
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDto.Type),
                    ProductKey = purchaseDto.Key,
                    Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Card = card,
                    CardId = card.Id,
                    Game = game,
                    GameId = game.Id
                };

                context.Purchases.Add(purchase);

                context.SaveChanges();

                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
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