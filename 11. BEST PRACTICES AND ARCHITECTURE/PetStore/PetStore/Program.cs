namespace PetStore
{
    using System;
    using System.Linq;
    using Petstore.Data;
    using Petstore.Data.Models;
    using PetStore.Services.Implementations;

    public class Program
    {
        public static void Main(string[] args)
        {
            using(var data = new PetstoreDbContext())
            {
                //var userService = new UserService(data);

                //var breedService = new BreedService(data);

                //var categoryService = new CategoryService(data);

                //var petservice = new PetService(data, breedService, categoryService, userService);

                //petservice.BuyPet(Gender.Male, DateTime.Now, 0m, null, 1, 1);

                //petservice.SellPet(1, 1);

                for (int i = 0; i < 10; i++)
                {
                    var breed = new Breed()
                    {
                        Name = "Breed " + i
                    };

                    data.Breeds.Add(breed);
                }

                data.SaveChanges();


                for (int i = 0; i < 30; i++)
                {
                    var category = new Category()
                    {
                        Name = "Category " + i,
                        Description = "Some random description" + i,
                    };

                    data.Categories.Add(category);
                }

                data.SaveChanges();

                for (int i = 0; i < 102; i++)
                {
                    var breedId = data.Breeds
                        .OrderBy(c => Guid.NewGuid())
                        .Select(c => c.Id)
                        .FirstOrDefault();

                    var categoryId = data.Categories
                       .OrderBy(c => Guid.NewGuid())
                       .Select(c => c.Id)
                       .FirstOrDefault();

                    var pet = new Pet()
                    {
                        DateOfBirth = DateTime.UtcNow.AddDays(-60 + i),
                        Price = 50 + i,
                        Gender = i % 2 == 0 ? Gender.Male : Gender.Female,
                        Description = "Some random description" + i,
                        CategoryId = categoryId,
                        BreedId = breedId
                    };

                    data.Pets.Add(pet);
                }

                data.SaveChanges();
            }
        }
    }
}
