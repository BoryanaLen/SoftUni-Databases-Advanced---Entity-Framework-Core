using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            Mapper.Initialize(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });


            context.Database.EnsureCreated();

            string inputXml = File.ReadAllText("../../../Datasets/users.xml");

            System.Console.WriteLine(ImportUsers(context, inputXml));
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("Users"));

            var usersDto = (UserDto[])ser.Deserialize(new StringReader(inputXml));

            List<User> users = new List<User>();

            foreach (var userDto in usersDto)
            {
                var user = Mapper.Map<User>(userDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}