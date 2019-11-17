using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = $"{s.Discount :F2}",
                    price = $"{s.Car.PartCars.Sum(x => x.Part.Price):F2}",
                    priceWithDiscount = $"{s.Car.PartCars.Sum(x => x.Part.Price)*(100 - s.Discount)/100:F2}"
                })
               .ToList();

            var result = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return result;
        }
    }
}