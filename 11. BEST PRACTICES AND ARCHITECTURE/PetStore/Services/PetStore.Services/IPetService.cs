using Petstore.Data.Models;
using PetStore.Services.Models.Pet;
using System;
using System.Collections.Generic;

namespace PetStore.Services
{
    public interface IPetService
    {
        IEnumerable<PetListingServiceModel> All(int page = 1);

        PetDetailsServiceModel Details(int id);

        void BuyPet(Gender gender, DateTime dateTime, decimal price, string description,
            int breedId, int categoryId);

        void SellPet(int petId, int userId);

        bool Exists(int petId);

        public int Total();
        bool Delete(int id);
    }
}
