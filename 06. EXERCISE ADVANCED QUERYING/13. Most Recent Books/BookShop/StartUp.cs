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
                Console.WriteLine(GetMostRecentBooks(db));
            }
        }


        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Select(c => new {
                    Name = c.Name,
                    Books = c.CategoryBooks
                    .Where(b => b.Book.ReleaseDate != null)
                    .Select(b => new { 
                        Title = b.Book.Title,
                        Date = b.Book.ReleaseDate
                     })
                    .OrderByDescending(b => b.Date)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({(book.Date.Value.Year)})");
                }

            }

            return sb.ToString().Trim();
        }
    }
}

