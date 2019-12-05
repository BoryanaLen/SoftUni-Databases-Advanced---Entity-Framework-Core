namespace PetClinic.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.Dtos.Export;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animals = context.Animals
                .Where(a => a.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(a => new
                {
                    OwnerName = a.Passport.OwnerName,
                    AnimalName = a.Name,
                    Age = a.Age,
                    SerialNumber = a.PassportSerialNumber,
                    RegisteredOn = a.Passport.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                })
                .OrderBy(a => a.Age)
                .ThenBy(a => a.SerialNumber)
                .ToList();

            var result = JsonConvert.SerializeObject(animals, Formatting.Indented);

            return result;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procedures = context.Procedures
                .Select(p => new ProcedureExportDto
                {
                    Passport = p.Animal.PassportSerialNumber,
                    OwnerNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = p.ProcedureAnimalAids.Select(aa => new AnimalAidExportDto
                    {
                        Name = aa.AnimalAid.Name,
                        Price = aa.AnimalAid.Price
                    })
                    .ToArray(),
                    TotalPrice = p.ProcedureAnimalAids.Select(pa => pa.AnimalAid).Sum(a => a.Price)
                })
                .OrderBy(p => DateTime.ParseExact(p.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .ThenBy(p => p.Passport)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(ProcedureExportDto[]), new XmlRootAttribute("Procedures"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), procedures, namespaces);

            string result = sb.ToString().TrimEnd();

            return result;
        }
    }
}
