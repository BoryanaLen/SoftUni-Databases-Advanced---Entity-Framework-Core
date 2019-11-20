using CarDealer.Data;
using CarDealer.Dtos.Export;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }


        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SalesWithAppliedDiscountDto
                {
                    Car = new CarsWithDistanceDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = ((s.Car.PartCars.Sum(pc => pc.Part.Price)) * (100 - s.Discount) / 100)
                })
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(SalesWithAppliedDiscountDto[]), new XmlRootAttribute("sales"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().TrimEnd();
        }

    }
}