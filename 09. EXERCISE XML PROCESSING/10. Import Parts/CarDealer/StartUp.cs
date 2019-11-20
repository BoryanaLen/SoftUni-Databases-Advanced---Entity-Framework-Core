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

            string inputXml = File.ReadAllText("../../../Datasets/parts.xml");

            Console.WriteLine(ImportParts(context, inputXml));
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(PartImportDto[]), new XmlRootAttribute("Parts"));

            var allPartsDto = (PartImportDto[])ser.Deserialize(new StringReader(inputXml));

            var partsDto = allPartsDto.Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId)).ToArray();

            List<Part> parts = new List<Part>();

            foreach (var partDto in partsDto)
            {
                var part = Mapper.Map<Part>(partDto);

                parts.Add(part);
            }

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }
    }
}