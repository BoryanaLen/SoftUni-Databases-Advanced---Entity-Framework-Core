namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                string command = Console.ReadLine().ToLower();

                Console.WriteLine(GetBooksByAgeRestriction(db, command));
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            var titles = context.Books
                .Where(b => b.AgeRestriction.ToString().ToLower().Equals(command, StringComparison.OrdinalIgnoreCase))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().Trim();
        }
    }
}

