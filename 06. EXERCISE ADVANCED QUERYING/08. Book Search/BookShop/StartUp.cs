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

                Console.WriteLine(GetBookTitlesContaining(db, input));
            }
        }


        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Select(b => b.Title)
                .Where(b => b.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(b => b)
                .ToList();

            books.ForEach(b => sb.AppendLine(b));

            return sb.ToString().Trim();
        }
    }
}

