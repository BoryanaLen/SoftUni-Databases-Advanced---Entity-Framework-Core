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
                context.Database.EnsureCreated();

                string inputJson = File.ReadAllText("../../../Datasets/suppliers.json");

                Console.WriteLine(ImportSuppliers(context, inputJson));
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var supplierList = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(supplierList);

            context.SaveChanges();

            return $"Successfully imported {supplierList.Count}.";
        }
    }
}