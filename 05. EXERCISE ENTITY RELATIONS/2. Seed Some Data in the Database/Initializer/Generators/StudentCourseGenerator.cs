using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem.Initializer.Generators
{
    internal class StudentCourseGenerator
    {
        internal static StudentCourse[] CreateStudentCourses()
        {
            string[] studentCoursesInput = new string[]
            {
                //StudentId,  CourseId
                "1 2",
                "2 1",
                "3 4",
                "5 3",
                "4 5"
            };


            int studentCoursesCount = studentCoursesInput.Length;

            StudentCourse[] studentCourses = new StudentCourse[studentCoursesCount];

            for (int i = 0; i < studentCoursesCount; i++)
            {
                string[] studentTokens = studentCoursesInput[i].Split();

                int studentId = int.Parse(studentTokens[0]);
                int courseId = int.Parse(studentTokens[1]);


                StudentCourse studentCourse = new StudentCourse()
                {
                    StudentId = studentId,
                    CourseId = courseId
                };

                studentCourses[i] = studentCourse;
            }

            return studentCourses;
        }
    }
}
