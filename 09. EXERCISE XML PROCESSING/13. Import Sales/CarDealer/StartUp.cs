using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            Mapper.Initialize(x =>
            {
                x.AddProfile<CarDealerProfile>();
            });

            string inputXml = File.ReadAllText("../../../Datasets/sales.xml");

            Console.WriteLine(ImportSales(context, inputXml));
        }


        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SaleImportDto[]), new XmlRootAttribute("Sales"));

            var allSalesDto = (SaleImportDto[])ser.Deserialize(new StringReader(inputXml));

            var salesDto = allSalesDto
                .Where(s => context.Cars.Any(c => c.Id == s.CarId));     

            List<Sale> sales = new List<Sale>();

            foreach (var saleDto in salesDto)
            {
                var sale = Mapper.Map<Sale>(saleDto);

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CustomerImportDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (CustomerImportDto[])ser.Deserialize(new StringReader(inputXml));

            List<Customer> customers = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                var customer = Mapper.Map<Customer>(customerDto);

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CarImportDto[]), new XmlRootAttribute("Cars"));

            var carsDto = (CarImportDto[])ser.Deserialize(new StringReader(inputXml));

            List<Car> cars = new List<Car>();

            foreach (var cardto in carsDto)
            {
                var car = Mapper.Map<Car>(cardto);

                var uniqueIds = cardto.Parts.PartsId.Select(id => id.PartId).Distinct().ToList();

                foreach (var id in uniqueIds)
                {
                    if (context.Parts.Any(p => p.Id == id))
                    {
                        var partCar = new PartCar
                        {
                            CarId = car.Id,
                            PartId = id
                        };

                        car.PartCars.Add(partCar);
                    }
                }

                cars.Add(car);

            }

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
    }
}