using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
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
                string inputJson = File.ReadAllText("../../../Datasets/parts.json");

                Console.WriteLine(ImportParts(context, inputJson));
            }
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var partsList = JsonConvert.DeserializeObject<List<Part>>(inputJson)
                .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                .ToList();

            context.Parts.AddRange(partsList);

            context.SaveChanges();

            return $"Successfully imported {partsList.Count}.";
        }
    }
}