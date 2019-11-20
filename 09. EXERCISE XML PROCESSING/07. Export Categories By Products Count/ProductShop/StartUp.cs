using ProductShop.Data;
using ProductShop.Dtos.Export;
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

            System.Console.WriteLine(GetCategoriesByProductsCount(context));
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoryProductsDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();


            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(CategoryProductsDto[]), new XmlRootAttribute("Categories"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().Trim();
        }
    }
}