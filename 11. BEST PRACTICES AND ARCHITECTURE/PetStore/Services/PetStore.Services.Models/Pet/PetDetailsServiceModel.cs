using Petstore.Data.Models;
using System;

namespace PetStore.Services.Models.Pet
{
    public class PetDetailsServiceModel
    {
        public int Id { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Breed { get; set; }
        public string Category { get; set; }
    }
}
