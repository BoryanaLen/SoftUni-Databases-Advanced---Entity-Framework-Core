using System;
using System.Collections.Generic;
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
                string inputJson = File.ReadAllText("../../../Datasets/cars.json");

                Console.WriteLine(ImportCars(context, inputJson));
            }
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsList = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);

            var cars = new List<Car>();

            var carParts = new List<PartCar>();

            foreach (var carDto in carsList)
            {
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                foreach (var part in carDto.PartsId.Distinct())
                {
                    var carPart = new PartCar()
                    {
                        PartId = part,
                        Car = car
                    };

                    carParts.Add(carPart);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);

            context.PartCars.AddRange(carParts);

            context.SaveChanges();

            return $"Successfully imported {carsList.Count}.";
        }
    }
}