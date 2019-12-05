using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var employeesList = JsonConvert.DeserializeObject<List<EmployeeImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var employeeDto in employeesList)
            {
                if (!IsValid(employeeDto) || employeeDto.Position.Length < 3 || employeeDto.Position.Length > 30)
                {
                    sb.AppendLine(FailureMessage);

                    continue;
                }

                var position = context.Positions.FirstOrDefault(p => p.Name == employeeDto.Position);

                if (position == null)
                {
                    position = new Position
                    {
                        Name = employeeDto.Position
                    };

                    context.Positions.Add(position);

                    context.SaveChanges();
                }

                Employee employee = new Employee
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position,
                    PositionId = position.Id
                };

                context.Employees.Add(employee);

                sb.AppendLine(string.Format(SuccessMessage, employee.Name));

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var itemsList = JsonConvert.DeserializeObject<List<ItemImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var itemDto in itemsList)
            {
                var itemFound = context.Items.FirstOrDefault(i => i.Name == itemDto.Name);

                if (!IsValid(itemDto) || itemDto.Category.Length < 3 || itemDto.Category.Length > 30
                    || itemFound != null)
                {
                    sb.AppendLine(FailureMessage);

                    continue;
                }

                var category = context.Categories.FirstOrDefault(c => c.Name == itemDto.Category);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = itemDto.Category
                    };

                    context.Categories.Add(category);

                    context.SaveChanges();
                }

                Item item = new Item
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category,
                    CategoryId = category.Id
                };

                context.Items.Add(item);

                sb.AppendLine(string.Format(SuccessMessage, item.Name));

                context.SaveChanges();

            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(OrderImportDto[]), new XmlRootAttribute("Orders"));

            var ordersDto = (OrderImportDto[])ser.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();

            foreach (var orderDto in ordersDto)
            {
                var employee = context.Employees.FirstOrDefault(e => e.Name == orderDto.Employee);

                string[] itemsNames = orderDto.Items.Select(i => i.Name).ToArray();

                if (!IsValid(orderDto) || employee == null
                    || !itemsNames.All(a => context.Items.Any(x => x.Name == a))
                    || !Enum.IsDefined(typeof(OrderType), orderDto.Type))
                {
                    sb.AppendLine(FailureMessage);

                    continue;
                }

                Order order = new Order
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Type = (OrderType)Enum.Parse(typeof(OrderType), orderDto.Type)
                };


                foreach (var itemDto in orderDto.Items)
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == itemDto.Name);

                    OrderItem orderItem = new OrderItem
                    {
                        Item = item,
                        ItemId = item.Id,
                        Order = order,
                        OrderId = order.Id,
                        Quantity = itemDto.Quantity
                    };

                    context.OrderItems.Add(orderItem);

                    order.OrderItems.Add(orderItem);
                }

                context.Orders.Add(order);

                sb.AppendLine($"Order for {order.Customer} on {order.DateTime.ToString("dd/MM/yyyy HH:mm")} added");

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);

            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}