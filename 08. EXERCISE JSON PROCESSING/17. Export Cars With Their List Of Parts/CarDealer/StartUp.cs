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
                Console.WriteLine(GetCarsWithTheirListOfParts(context));
            }
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new PartCarDto
                {
                    Car = new CarDto()
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    Parts = c.PartCars.Select(cp => new PartDto
                    {
                        Name = cp.Part.Name,
                        Price = $"{cp.Part.Price:F2}"
                    })
                    .ToList()

                })
                .ToList();


            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }
    }
}