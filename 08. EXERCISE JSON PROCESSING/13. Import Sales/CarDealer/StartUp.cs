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
                string inputJson = File.ReadAllText("../../../Datasets/sales.json");

                Console.WriteLine(ImportSales(context, inputJson));
            }
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var salesList = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(salesList);

            context.SaveChanges();

            return $"Successfully imported {salesList.Count}.";
        }
    }
}