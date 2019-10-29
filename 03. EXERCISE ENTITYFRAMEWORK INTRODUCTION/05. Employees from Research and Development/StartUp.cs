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
                Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            }
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            int departmentId = context.Departments
                .Where(d => d.Name == "Research and Development")
                .Select(d => d.DepartmentId)
                .FirstOrDefault();

            var employees = context.Employees
                .Where(e => e.DepartmentId == departmentId)
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (Employee empl in employees)
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} from Research and Development - ${empl.Salary:F2}");
            }

            return sb.ToString().Trim();
        }
    }
}
