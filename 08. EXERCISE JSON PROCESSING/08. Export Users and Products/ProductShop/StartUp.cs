using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using System;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(u => new UserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ProductSoldDto()
                    {
                        Count = u.ProductsSold.Where(ps => ps.Buyer != null).Count(),
                        Products = u.ProductsSold
                                .Where(ps => ps.Buyer != null)
                                .Select(p => new ProductDto()
                                {
                                    Name = p.Name,
                                    Price = p.Price
                                })
                                .ToList()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var usersDto = new
            {
                usersCount = users.Count(),
                users
            };

            string result = JsonConvert.SerializeObject(usersDto, Formatting.Indented);

            int length = result.Length;

            return result;
        }
    }
}