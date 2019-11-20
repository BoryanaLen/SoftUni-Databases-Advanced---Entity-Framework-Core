using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.IO;
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

            string inputXml = File.ReadAllText("../../../Datasets/categories.xml");

            System.Console.WriteLine(ImportCategories(context, inputXml));
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));

            var categoriesDto = (CategoryDto[])ser.Deserialize(new StringReader(inputXml));

            List<Category> categories = new List<Category>();

            foreach (var categoryDto in categoriesDto)
            {
                var category  = Mapper.Map<Category>(categoryDto);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
    }
}