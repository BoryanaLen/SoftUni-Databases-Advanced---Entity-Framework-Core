using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            var inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            Console.WriteLine(ImportCategoryProducts(context, inputJson));
        }

 

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var cp = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson).ToList();

            context.CategoryProducts.AddRange(cp);

            context.SaveChanges();

            return $"Successfully imported {cp.Count}";
        }
    }
}