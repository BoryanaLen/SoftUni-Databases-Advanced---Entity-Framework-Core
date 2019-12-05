namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    Name = p.FullName,
                    p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                    .Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(o => o.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(po => po.Officer.Salary), 2)
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();
                

            var result = JsonConvert.SerializeObject(prisoners, Newtonsoft.Json.Formatting.Indented);

            return result;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            List<string> names = prisonersNames.Split(",").ToList();

            var prisoners = context.Prisoners
                .Where(p => names.Contains(p.FullName))
                .Select(p => new PrisonerExportDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails
                        .Select(m => new MessageExportDto
                        {
                            Description = Reverse(m.Description)
                        })
                         .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer ser = new XmlSerializer(typeof(PrisonerExportDto[]), new XmlRootAttribute("Prisoners"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            ser.Serialize(new StringWriter(sb), prisoners, namespaces);

            string result = sb.ToString().TrimEnd();

            return result;
        }


        private static string Reverse(string description)
        {
            if (description == null) return null;

            char[] array = description.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }

    }
}