using P01_StudentSystem.Data.Models;
using System;
using System.Globalization;

namespace P01_StudentSystem.Initializer.Generators
{
    internal class CourseGenerator
    {
        internal static Course[] CreateCourses()
        {
            string[] coursesInput = new string[]
            {
                //Id Description, EndDate, Name, StartDate, Price},
                "1 Description-1 15/07/2008 C#-1 15/06/2008 100",
                "2 Description-2 15/07/2008 C#-2 15/06/2008 100",
                "3 Description-3 15/07/2008 C#-3 15/06/2008 100",
                "4 Description-4 15/07/2008 C#-4 15/06/2008 100",
                "5 Description-5 15/07/2008 C#-5 15/06/2008 100",
            };


            int coursesCount = coursesInput.Length;

            Course[] courses = new Course[coursesCount];

            for (int i = 0; i < coursesCount; i++)
            {
                string[] courseTokens = coursesInput[i].Split();

                int id = int.Parse(courseTokens[0]);
                string description = courseTokens[1];
                string endDate = courseTokens[2];
                string name = courseTokens[3];
                string startDate = courseTokens[4];
               decimal price = decimal.Parse(courseTokens[5]);
               

                Course course = new Course()
                {
                    CourseId = id,
                    Description = description,
                    EndDate = Convert.ToDateTime(endDate),
                    Name = name,
                    StartDate = Convert.ToDateTime(startDate),
                    Price = price
                };

                courses[i] = course;
            }

            return courses;
        }
    }
}
