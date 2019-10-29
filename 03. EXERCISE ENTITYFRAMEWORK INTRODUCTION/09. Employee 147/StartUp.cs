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
                Console.WriteLine(GetEmployee147(context));
            }
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Employee employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .FirstOrDefault();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            var projects = context.EmployeesProjects
                .Where(x => x.EmployeeId == 147)
                .Select(x => x.Project.Name)
                .OrderBy(x => x)
                .ToList();

            foreach (var pr in projects)
            {
                sb.AppendLine($"{pr}");
            }

            return sb.ToString().Trim();
        }
    }
}
