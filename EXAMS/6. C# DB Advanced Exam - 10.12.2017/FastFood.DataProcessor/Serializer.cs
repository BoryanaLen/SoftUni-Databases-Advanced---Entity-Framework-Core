using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
		public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
		{
            Employee employee = context.Employees
                .Where(e => e.Name == employeeName).FirstOrDefault();

            var orders = employee.Orders
                .Where(o => o.Type.ToString() == orderType)
                .Select(o => new
                {
                    Customer = o.Customer,
                    Items = o.OrderItems.Select(oi => new
                    {
                        Name = oi.Item.Name,
                        Price = oi.Item.Price,
                        Quantity = oi.Quantity
                    })
                    .ToArray(),
                    TotalPrice = o.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity)
                })
                .OrderByDescending(o => o.TotalPrice)
                .ThenByDescending(o => o.Items.Count())
                .ToArray();

            var employeeOrders = new
            {
                Name = employeeName,
                Orders = orders,
                TotalMade = orders.Sum(o => o.TotalPrice)
            };

            var result = JsonConvert.SerializeObject(employeeOrders, Formatting.Indented);

            return result;
        }

		public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
            string[] categoriesNames = categoriesString.Split(',').ToArray();

            var categories = context.Categories
                .Where(c => categoriesNames.Any(n => c.Name == n))
                .Select(c => new CategoryExportDto
                {
                    Name = c.Name,
                    MostPopularItem = new ItemExportDto
                    {
                        Name = c.Items
                        .OrderByDescending(i => i.Price  * i.OrderItems.Sum(oi => oi.Quantity))
                        .FirstOrDefault().Name,

                        TotalMade = c.Items
                        .OrderByDescending(i => i.Price * i.OrderItems.Sum(oi => oi.Quantity))
                        .Select(i => i.Price * i.OrderItems.Sum(oi => oi.Quantity))
                        .FirstOrDefault(),

                        TimesSold = c.Items
                        .OrderByDescending(i => i.Price * i.OrderItems.Sum(oi => oi.Quantity))
                        .Select(i => i.OrderItems.Sum(oi => oi.Quantity))
                        .FirstOrDefault()
                    }
                })
                .OrderByDescending(c => c.MostPopularItem.TotalMade)
                .ThenByDescending(c => c.MostPopularItem.TimesSold)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(CategoryExportDto[]), new XmlRootAttribute("Categories"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), categories, namespaces);

            string result = sb.ToString().TrimEnd();

            return result;
        }
	}
}