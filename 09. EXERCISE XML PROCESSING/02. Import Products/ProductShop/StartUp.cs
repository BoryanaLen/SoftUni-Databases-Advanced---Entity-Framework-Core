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

            string inputXml = File.ReadAllText("../../../Datasets/products.xml");

            System.Console.WriteLine(ImportProducts(context, inputXml));
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("Products"));

            var productsDto = (ProductDto[])ser.Deserialize(new StringReader(inputXml));

            List<Product> products = new List<Product>();

            foreach (var productDto in productsDto)
            {
                var product = Mapper.Map<Product>(productDto);
                products.Add(product);
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }
    }
}