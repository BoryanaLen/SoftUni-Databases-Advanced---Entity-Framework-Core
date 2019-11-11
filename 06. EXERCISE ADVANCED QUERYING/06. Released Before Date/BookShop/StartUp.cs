namespace BookShop
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                string input = Console.ReadLine();

                Console.WriteLine(GetBooksReleasedBefore(db, input));
            }
        }


        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();

            DateTime formatedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate != null && b.ReleaseDate.Value < formatedDate)
                .OrderByDescending(b => b.ReleaseDate.Value)
                .Select(b => new {
                    Title = b.Title,
                    EditionType = b.EditionType,
                    Price = b.Price
                })               
                .ToList();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:F2}");
            }   

            return sb.ToString().Trim();
        }
    }
}

