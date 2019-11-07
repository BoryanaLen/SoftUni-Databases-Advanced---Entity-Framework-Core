using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem.Initializer.Generators
{
    internal class HomeworkGenerator
    {
        internal static Homework[] CreateHomeworks()
        {
            string[] homeworksInput = new string[]
            {
                //Id Content, ContentType, SubmissionTime, CourseId, StudentId
                "1 Content-1 Application 1 1",
                "2 Content-2 Zip 2 2",
                "3 Content-3 Pdf 3 3",
                "4 Content-4 Zip 4 4",
                "5 Content-5 Application 5 5"
            };


            int homeworksCount = homeworksInput.Length;

            Homework[] homeworks = new Homework[homeworksCount];

            for (int i = 0; i < homeworksCount; i++)
            {
                string[] homeworkTokens = homeworksInput[i].Split();

                int id = int.Parse(homeworkTokens[0]);
                string content = homeworkTokens[1];
                string contentType = homeworkTokens[2];
                int courseId = int.Parse(homeworkTokens[3]);
                int studentId = int.Parse(homeworkTokens[4]);

                if (!Enum.TryParse(content, out ContentType type))
                {
                    contentType = "Application";
                }

                Homework homework = new Homework()
                {
                    HomeworkId = id,
                    Content = content,
                    ContentType = (ContentType)Enum.Parse(typeof(ContentType), contentType, true),
                    SubmissionTime = DateTime.Now,
                    CourseId = courseId,
                    StudentId = studentId
                };

                homeworks[i] = homework;
            }

            return homeworks;
        }
    }
}
