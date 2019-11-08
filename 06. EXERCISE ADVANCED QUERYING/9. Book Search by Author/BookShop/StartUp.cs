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

                Console.WriteLine(GetBooksByAuthor(db, input));
            }
        }


        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Author.LastName.StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(b => b.BookId)
                .Select(b => new { 
                    Title = b.Title,
                    AuthorName = b.Author.FirstName + " " + b.Author.LastName
                    })
                .ToList();

            books.ForEach(b => sb.AppendLine($"{b.Title} ({b.AuthorName})"));

            return sb.ToString().Trim();
        }
    }
}

