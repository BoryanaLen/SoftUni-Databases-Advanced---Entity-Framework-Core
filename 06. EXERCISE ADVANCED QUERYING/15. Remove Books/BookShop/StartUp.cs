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
                Console.WriteLine(RemoveBooks(db));
            }
        }


        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
              .Where(b => b.Copies < 4200)
              .ToList();

            foreach (var book in books)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            int count = books.Count;

            return count;
        }
    }
}

