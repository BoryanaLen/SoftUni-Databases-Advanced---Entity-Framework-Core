using P01_StudentSystem.Data.Models;
using System;


namespace P01_StudentSystem.Initializer.Generators
{
    internal class StudentGenerator
    {
        internal static Student[] CreateStudents()
        {
            string[] studentsInput = new string[]
            {
                //Birthday, Name, PhoneNumber,  RegisteredOn,
                "1 15/10/1990 Ivan 0888123456 15/07/2008",
                "2 16/10/1990 Dimitar 0888123456 16/07/2008",
                "3 17/10/1990 Petar 0888123456 17/07/2008",
                "4 18/10/1990 George 0888123456 18/07/2008",
                "5 19/10/1990 Martin 0888123456 19/07/2008"
            };


            int studentsCount = studentsInput.Length;

            Student[] students = new Student[studentsCount];

            for (int i = 0; i < studentsCount; i++)
            {
                string[] studentTokens = studentsInput[i].Split();

                int id = int.Parse(studentTokens[0]);
                string birthday = studentTokens[1];
                string name = studentTokens[2];
                string phoneNumber = studentTokens[3];
                string registeredOn = studentTokens[4];


                Student student = new Student()
                {
                   StudentId = id,
                   Birthday = Convert.ToDateTime(birthday),
                   Name = name,
                   PhoneNumber = phoneNumber,
                   RegisteredOn = Convert.ToDateTime(registeredOn)
                };

                students[i] = student;
            }

            return students;
        }
    }
}
