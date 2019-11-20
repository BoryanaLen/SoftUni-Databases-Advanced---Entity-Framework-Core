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

            Console.WriteLine(GetCarsWithDistance(context));
        }


        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
              .Where(c => c.TravelledDistance > 2000000)
              .Select(c => new CarsWithDistanceDto
              {
                  Make = c.Make,
                  Model = c.Model,
                  TravelledDistance = c.TravelledDistance
              })
              .OrderBy(c => c.Make)
              .ThenBy(c => c.Model)
              .Take(10)
              .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(CarsWithDistanceDto[]), new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().Trim();
        }
    }
}