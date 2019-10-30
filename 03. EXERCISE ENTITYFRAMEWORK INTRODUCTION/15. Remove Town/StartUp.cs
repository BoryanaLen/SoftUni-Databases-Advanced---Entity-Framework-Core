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
                Console.WriteLine(RemoveTown(context));
            }
        }

        public static string RemoveTown(SoftUniContext context)
        {
            string townName = "Seattle";

            int townId = context.Towns
                .Where(t => t.Name == townName)
                .Select(t => t.TownId)
                .FirstOrDefault();

            Town town = context.Towns
                .Where(t => t.Name == townName)
                .FirstOrDefault();

            var employees = context.Employees
                .Where(e => e.Address.Town.Name == townName)
                .ToList();

            foreach (var empl in employees)
            {
                empl.AddressId = null;
                context.SaveChanges();
            }

            var addresses = context.Addresses
                .Where(a => a.TownId == townId)
                .ToList();

            int countAddresses = addresses.Count;

            foreach (var address in addresses)
            {
                context.Remove(address);
                context.SaveChanges();
            }

            context.Remove(town);
            context.SaveChanges();

            return $"{countAddresses} addresses in Seattle were deleted";
        }
    }
}
