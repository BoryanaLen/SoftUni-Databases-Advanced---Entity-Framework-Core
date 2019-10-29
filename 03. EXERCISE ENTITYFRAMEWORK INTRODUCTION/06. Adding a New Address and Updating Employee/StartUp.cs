using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            using (context)
            {
                Console.WriteLine(AddNewAddressToEmployee(context));
            }
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address address = new Address();
            address.AddressText = "Vitoshka 15";
            address.TownId = 4;

            context.Addresses.Add(address);
            context.SaveChanges();

            int addressId = context.Addresses
                .Where(a => a.AddressText == "Vitoshka 15")
                .Select(a => a.AddressId)
                .FirstOrDefault();

            var employees = context.Employees
                .Where(e => e.LastName == "Nakov")
                .ToList();

            foreach (Employee empl in employees)
            {
                empl.AddressId = addressId;
                context.SaveChanges();
            }

            var allEmployees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToList();

            foreach (var empl in allEmployees)
            {
                sb.AppendLine($"{empl}");
            }

            return sb.ToString().Trim();
        }
    }
}
