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
                Console.WriteLine(GetGoldenBooks(db));
            }
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var titles = context.Books
                .Where(b => b.EditionType.ToString().Equals("Gold") && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }               

            return sb.ToString().Trim();
        }
    }
}

