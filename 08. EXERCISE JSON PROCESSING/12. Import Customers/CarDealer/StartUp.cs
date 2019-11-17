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
                string inputJson = File.ReadAllText("../../../Datasets/customers.json");

                Console.WriteLine(ImportCustomers(context, inputJson));
            }
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customersList = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customersList);

            context.SaveChanges();

            return $"Successfully imported {customersList.Count}.";
        }
    }
}