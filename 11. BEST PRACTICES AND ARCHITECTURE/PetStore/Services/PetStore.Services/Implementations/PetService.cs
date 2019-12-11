using System;
using System.Collections.Generic;
using System.Linq;
using Petstore.Data;
using Petstore.Data.Models;
using PetStore.Services.Models.Pet;

namespace PetStore.Services.Implementations
{
    public class PetService : IPetService
    {
        private readonly PetstoreDbContext data;

        private readonly IBreedService breedService;

        private readonly ICategoryService categoryService;

        private readonly IUserService userService;

        private const int PetsPageSize = 25;

        public PetService(PetstoreDbContext data, IBreedService breedService, 
            ICategoryService categoryService, IUserService userService)
        {
            this.data = data;
            this.breedService = breedService;
            this.categoryService = categoryService;
            this.userService = userService;
        }

        public IEnumerable<PetListingServiceModel> All(int page = 1)
        {
            return this.data.Pets
                .Skip((page -1) * PetsPageSize)
                .Take(PetsPageSize)
                .Select(p => new PetListingServiceModel
                {
                    Id = p.Id,
                    Price = p.Price,
                    Category = p.Category.Name,
                    Breed = p.Breed.Name
                });
        }

        public void BuyPet(Gender gender, DateTime dateofBirth, decimal price, string description, 
            int breedId, int categoryId)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price of the pet cannot be less than zero!");
            }

            if (!this.breedService.Exists(breedId))
            {
                throw new ArgumentException("There is no such breed with given id in database!");
            }

            if (!this.categoryService.Exists(categoryId))
            {
                throw new ArgumentException("There is no such category with given id in database!");
            }

            var pet = new Pet()
            {
                Gender = gender,
                DateOfBirth = dateofBirth,
                Price = price,
                Description = description,
                BreedId = breedId,
                CategoryId = categoryId
            };

            this.data.Pets.Add(pet);

            this.data.SaveChanges();
        }

        public bool Exists(int petId)
        {
            return this.data.Pets.Any(p => p.Id == petId);
        }

        public void SellPet(int petId, int userId)
        {
            if (!this.userService.Exists(userId))
            {
                throw new ArgumentException("There is no such user with given id in database!");
            }

            if (!this.Exists(petId))
            {
                throw new ArgumentException("There is no such pet with given id in database!");
            }

            var pet = this.data.Pets.First(p => p.Id == petId);

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = OrderStatus.Done,
                UserId = userId,
            };

            this.data.Orders.Add(order);

            pet.Order = order;

            this.data.SaveChanges();
        }

        public int Total() => this.data.Pets.Count();

        public PetDetailsServiceModel Details(int id)
            => this.data
                .Pets
                .Where(p => p.Id == id)
                .Select(p => new PetDetailsServiceModel
                {
                    Id = p.Id,
                    Breed = p.Breed.Name,
                    Category = p.Category.Name,
                    DateOfBirth = p.DateOfBirth,
                    Description = p.Description,
                    Gender = p.Gender,
                    Price = p.Price
                })
                .FirstOrDefault();

        public bool Delete(int id)
        {
            var pet = this.data.Pets.Find(id);

            if (pet == null)
            {
                return false;
            }

            this.data.Pets.Remove(pet);

            this.data.SaveChanges();

            return true;
        }
    }
}
