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

            var inputJson = File.ReadAllText("../../../Datasets/products.json");

            Console.WriteLine(ImportProducts(context, inputJson));
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var productsList = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(productsList);

            context.SaveChanges();

            return $"Successfully imported {productsList.Count}";
        }
    }
}