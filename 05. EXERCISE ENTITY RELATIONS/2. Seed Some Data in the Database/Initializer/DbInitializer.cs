

using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using P01_StudentSystem.Initializer.Generators;
using System;

namespace P01_StudentSystem.Initializer
{
    public class DbInitializer
    {
        public void ResetDatabase(StudentSystemContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("StydentSystem database created successfully.");

            Seed(context);
        }

        public static void Seed(StudentSystemContext context)
        {
            Course[] courses = CourseGenerator.CreateCourses();
            context.Courses.AddRange(courses);

            Student[] students = StudentGenerator.CreateStudents();
            context.Students.AddRange(students);

            Resource[] resources = ResourceGenerator.CreateResources();
            context.Resources.AddRange(resources);

            Homework[] homeworks = HomeworkGenerator.CreateHomeworks();
            context.HomeworkSubmissions.AddRange(homeworks);

            StudentCourse[] studentCourses = StudentCourseGenerator.CreateStudentCourses();
            context.StudentCourses.AddRange(studentCourses);

            context.SaveChanges();

            Console.WriteLine("Sample data inserted successfully.");
        }
    }
}

