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

            System.Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            int usersCount = context.Users
                .Where(u => u.ProductsSold.Count > 0).Count();

            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(p => p.ProductsSold.Count())
                .Select(u => new UserProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Products = new ProductsDto
                    {
                        Count = u.ProductsSold.Count(),
                        Products = u.ProductsSold
                        .Select(p => new SoldProductDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            var result = new UsersAndProductsDto
            {
                Count = usersCount,
                Users = users
            };

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(UsersAndProductsDto), new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), result, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}