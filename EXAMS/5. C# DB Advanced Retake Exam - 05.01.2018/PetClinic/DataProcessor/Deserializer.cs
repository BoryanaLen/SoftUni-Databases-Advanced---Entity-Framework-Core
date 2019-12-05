namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.Dtos.Import;
    using PetClinic.Models;

    public class Deserializer
    {

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var animalAidsList = JsonConvert.DeserializeObject<List<AnimalAidImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var animalAidDto in animalAidsList)
            {
                if (!IsValid(animalAidDto) || context.AnimalAids.Any(aa => aa.Name == animalAidDto.Name))
                {
                    sb.AppendLine("Error: Invalid data.");

                    continue;
                }

                AnimalAid animalAid = new AnimalAid
                {
                    Name = animalAidDto.Name,
                    Price = animalAidDto.Price
                };

                context.AnimalAids.Add(animalAid);

                sb.AppendLine($"Record {animalAid.Name} successfully imported.");

                context.SaveChanges();

            }

            return sb.ToString().TrimEnd();

        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var animalsList = JsonConvert.DeserializeObject<List<AnimalImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var animalDto in animalsList)
            {
                if (!IsValid(animalDto) || !IsValid(animalDto.Passport)
                    || context.Passports.Any(p => p.SerialNumber == animalDto.Passport.SerialNumber))
                {
                    sb.AppendLine("Error: Invalid data.");

                    continue;
                }

                Passport passport = new Passport
                {
                    SerialNumber = animalDto.Passport.SerialNumber,
                    OwnerName = animalDto.Passport.OwnerName,
                    OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                    RegistrationDate = DateTime.ParseExact(animalDto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                };

                Animal animal = new Animal
                {
                    Name = animalDto.Name,
                    Type = animalDto.Type,
                    Age = animalDto.Age,
                    Passport = passport
                };

                context.Passports.Add(passport);

                context.Animals.Add(animal);

                sb.AppendLine($"Record {animal.Name} Passport №: {passport.SerialNumber} successfully imported.");

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(VetImportDto[]), new XmlRootAttribute("Vets"));

            var vetsDto = (VetImportDto[])ser.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();

            foreach (var vetDto in vetsDto)
            {

                if (!IsValid(vetDto) || context.Vets.Any(v => v.PhoneNumber == vetDto.PhoneNumber))
                {
                    sb.AppendLine("Error: Invalid data.");

                    continue;
                }

                Vet vet = new Vet
                {
                    Name = vetDto.Name,
                    Profession = vetDto.Profession,
                    PhoneNumber = vetDto.PhoneNumber,
                    Age = vetDto.Age
                };

                context.Vets.Add(vet);

                sb.AppendLine($"Record {vet.Name} successfully imported.");

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ProcedureImportDto[]), new XmlRootAttribute("Procedures"));

            var proceduresDto = (ProcedureImportDto[])ser.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();

            foreach (var procedureDto in proceduresDto)
            {
                var animal = context.Animals.FirstOrDefault(a => a.PassportSerialNumber == procedureDto.Animal);

                var vet = context.Vets.FirstOrDefault(v => v.Name == procedureDto.Vet);

                string[] aidsNames = procedureDto.AnimalAids.Select(aa => aa.Name).ToArray();

                int distinctCount = aidsNames.Distinct().Count();

                int count = aidsNames.Count();

                if (!IsValid(procedureDto) || animal == null || vet == null || distinctCount != count
                    || !aidsNames.All(aa => context.AnimalAids.Any(x => x.Name == aa)))
                {
                    sb.AppendLine("Error: Invalid data.");

                    continue;
                }

                Procedure procedure = new Procedure
                {
                    Animal = animal,
                    Vet = vet,
                    DateTime = DateTime.ParseExact(procedureDto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                };

                foreach (var animalAidDto in procedureDto.AnimalAids)
                {
                    var animalAid = context.AnimalAids.FirstOrDefault(aa => aa.Name == animalAidDto.Name);

                    ProcedureAnimalAid procedureAnimalAid = new ProcedureAnimalAid
                    {
                        AnimalAid = animalAid,
                        AnimalAidId = animalAid.Id,
                        Procedure = procedure,
                        ProcedureId = procedure.Id
                    };

                    procedure.ProcedureAnimalAids.Add(procedureAnimalAid);

                    context.ProceduresAnimalAids.Add(procedureAnimalAid);
                }

                context.Procedures.Add(procedure);

                sb.AppendLine($"Record successfully imported.");

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
