using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            System.Console.WriteLine(GetProductsInRange(context));
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName              
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(ProductInRangeDto[]), new XmlRootAttribute("Products"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            ser.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().Trim();
        }
    }
}