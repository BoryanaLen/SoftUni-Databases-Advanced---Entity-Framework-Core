namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsList = JsonConvert.DeserializeObject<List<DepartmentImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var departmentDto in departmentsList)
            {
                if (!IsValid(departmentDto) || !departmentDto.Cells.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");

                    continue;
                }

                Department department = new Department
                {
                    Name = departmentDto.Name
                };

                List<Cell> cellsList = new List<Cell>();

                foreach (var cellDto in departmentDto.Cells)
                {
                    Cell cell = new Cell
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow,
                        Department = department,
                        DepartmentId = department.Id
                    };

                    cellsList.Add(cell);

                    department.Cells.Add(cell);
                }

                context.Departments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");

                context.Cells.AddRange(cellsList);
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonerssList = JsonConvert.DeserializeObject<List<PrisonerImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var prisonerDto in prisonerssList)
            {
                if (!IsValid(prisonerDto) || !prisonerDto.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");

                    continue;
                }

                Prisoner prisoner = new Prisoner
                {
                   FullName = prisonerDto.FullName,
                   Nickname = prisonerDto.Nickname,
                   Age = prisonerDto.Age,
                   Bail = prisonerDto.Bail,
                   CellId = prisonerDto.CellId
                };

                foreach (var mailDto in prisonerDto.Mails)
                {
                    Mail mail = new Mail
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address,
                        Prisoner = prisoner,
                        PrisonerId = prisoner.Id
                    };

                    context.Mails.Add(mail);

                    prisoner.Mails.Add(mail);

                }

                context.Prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");

                context.SaveChanges();

            }

            return sb.ToString().TrimEnd();

        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(OfficerImportDto[]), new XmlRootAttribute("Officers"));

            var officersDto = (OfficerImportDto[])ser.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();

            foreach (var officerDto in officersDto)
            {

                if (!IsValid(officerDto) || !Enum.IsDefined(typeof(Position), officerDto.Position)
                    || !Enum.IsDefined(typeof(Weapon), officerDto.Weapon))
                {
                    sb.AppendLine("Invalid Data");

                    continue;
                }

                Officer officer = new Officer
                {
                    FullName = officerDto.Name,
                    Salary = officerDto.Money,
                    Position = (Position)Enum.Parse(typeof(Position), officerDto.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), officerDto.Weapon),
                    DepartmentId = officerDto.DepartmentId
                };

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner
                    {
                        PrisonerId = prisonerDto.Id
                    };

                    officer.OfficerPrisoners.Add(officerPrisoner);

                    context.OfficersPrisoners.Add(officerPrisoner);
                }

                context.Officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);

            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}