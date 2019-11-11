namespace BookShop
{
    using Data;

    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                int year = int.Parse(Console.ReadLine());

                Console.WriteLine(GetBooksNotReleasedIn(db, year));
            }
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var titles = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().Trim();
        }

        //public static string GetBooksByPrice(BookShopContext context)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    var titles = context.Books
        //        .Where(b => b.Price > 40)
        //        .OrderByDescending(b => b.Price)
        //        .Select(b => $"{b.Title} - ${b.Price}")
        //        .ToList();

        //    foreach (var title in titles)
        //    {
        //        sb.AppendLine(title);
        //    }               

        //    return sb.ToString().Trim();
        //}
    }
}

