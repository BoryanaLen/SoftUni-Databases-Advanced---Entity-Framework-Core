using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            context.Database.EnsureCreated();

            Mapper.Initialize(x =>
            {
                x.AddProfile<CarDealerProfile>();
            });

            string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");

            Console.WriteLine(ImportSuppliers(context, inputXml));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SupplierImportDto[]), new XmlRootAttribute("Suppliers"));

            var suppliersDto = (SupplierImportDto[])ser.Deserialize(new StringReader(inputXml));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplierDto in suppliersDto)
            {
                var supplier = Mapper.Map<Supplier>(supplierDto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }
    }
}