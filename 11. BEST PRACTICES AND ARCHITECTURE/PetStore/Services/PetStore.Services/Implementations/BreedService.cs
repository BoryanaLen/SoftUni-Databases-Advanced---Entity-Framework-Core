using Petstore.Data;
using Petstore.Data.Models;
using System;
using System.Linq;

namespace PetStore.Services.Implementations
{
    public class BreedService : IBreedService
    {

        private readonly PetstoreDbContext data;

        public BreedService(PetstoreDbContext data)
        {
            this.data = data;
        }

        public void Add(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Breed name cannot be null or whitespace!");
            }

            var breed = new Breed()
            {
                Name = name
            };

            this.data.Breeds.Add(breed);
            this.data.SaveChanges();
        }

        public bool Exists(int breedId)
        {
            return this.data.Breeds
                 .Any(b => b.Id == breedId);
        }
    }
}
