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
                Console.WriteLine(DeleteProjectById(context));
            }
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var emplProjectToRemove = context.EmployeesProjects
                .Where(x => x.ProjectId == 2)
                .ToList();

            foreach(var entity in emplProjectToRemove)
            {
                context.EmployeesProjects.Remove(entity);
                context.SaveChanges();
            }

            Project projectToRemove = context.Projects.Where(p => p.ProjectId == 2).FirstOrDefault();

            context.Projects.Remove(projectToRemove);
            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .ToList();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
            }

            return sb.ToString().Trim();
        }
    }
}
