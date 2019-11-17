using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            Console.WriteLine(GetCategoriesByProductsCount(context));
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoryDto
                {
                    Name = c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = Math.Round(c.CategoryProducts.Average(cp => cp.Product.Price), 2).ToString(),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString()
                })
                .OrderByDescending(cp => cp.ProductsCount)
                .ToList();

            string result = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return result;
        }
    }
}