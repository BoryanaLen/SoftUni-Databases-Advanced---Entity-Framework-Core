using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem.Initializer.Generators
{
    internal class ResourceGenerator
    {
        internal static Resource[] CreateResources()
        {
            string[] resourcesInput = new string[]
            {
                //id CourseId, Name, ResourceType, Url" 
                "1 1 Name-1 C#-1 Video www.softuni.bg/ef-core",
                "2 2 Name-2 C#-1 Document www.softuni.bg/ef-core",
                "3 3 Name-3 C#-1 Video www.softuni.bg/ef-core",
                "4 4 Name-4 C#-1 Document www.softuni.bg/ef-core",
                "5 5 Name-5 C#-1 Video www.softuni.bg/ef-core",
            };


            int resourcesCount = resourcesInput.Length;

            Resource[] resources = new Resource[resourcesCount];

            for (int i = 0; i < resourcesCount; i++)
            {
                string[] resourceTokens = resourcesInput[i].Split();

                int Id = int.Parse(resourceTokens[0]);
                int courseId = int.Parse(resourceTokens[1]);
                string name = resourceTokens[2];
                string resourceType = resourceTokens[3];
                string url = resourceTokens[4];


                if (!Enum.TryParse(resourceType, out ResourceType type))
                {
                    resourceType = "Document";
                }

                Resource resource = new Resource()
                {
                    ResourceId = Id,
                    CourseId = courseId,
                    Name = name,
                    ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), resourceType, true),
                    Url = url
                };

                resources[i] = resource;
            }

            return resources;
        }
    }
}
