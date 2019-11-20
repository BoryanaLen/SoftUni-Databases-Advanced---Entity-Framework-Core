using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            System.Console.WriteLine(ImportCategoryProducts(context, inputXml));
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CategoryProductDto[]), new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDto = (CategoryProductDto[])ser.Deserialize(new StringReader(inputXml));

            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDto)
            {
                var categoryProduct  = Mapper.Map<CategoryProduct>(categoryProductDto);
                categoryProducts.Add(categoryProduct);
            }

            var categoryProductsToAdd = categoryProducts
                .Where(cp => context.Categories.Any(c => c.Id == cp.CategoryId)
                    && context.Products.Any(p => p.Id == cp.ProductId))
                .ToList();
                    

            context.CategoryProducts.AddRange(categoryProductsToAdd);
            context.SaveChanges();

            return $"Successfully imported {categoryProductsToAdd.Count}";
        }
    }
}