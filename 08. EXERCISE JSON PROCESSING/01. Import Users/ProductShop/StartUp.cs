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

            var inputJson = File.ReadAllText("../../../Datasets/users.json");

            Console.WriteLine(ImportUsers(context, inputJson));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var usersList = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(usersList);

            context.SaveChanges();

            return $"Successfully imported {usersList.Count}";
        }
    }
}