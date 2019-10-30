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
                Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            }
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(x => new
                {
                    DepartmentName = x.Name,
                    ManagerFullName = x.Manager.FirstName + " " + x.Manager.LastName,
                    DepEmployees = x.Employees
                    .Select(y => new { 
                        FirstName = y.FirstName,
                        LastName = y.LastName,
                        JobTitle = y.JobTitle
                    })
                    .OrderBy(y => y.FirstName)
                    .ThenBy(y => y.LastName)
                })
                .ToList();

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.DepartmentName} - {dep.ManagerFullName}");

                foreach(var empl in dep.DepEmployees)
                {
                    sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle}");
                }
            }

            return sb.ToString().Trim();
        }
    }
}
