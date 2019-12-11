using System.Collections.Generic;

namespace PetStore.Services.Models.Brand
{
    using Toy;
    public class BrandWithToysServiceModel
    {
        public string Name { get; set; }

        public IEnumerable<ToyListingServiceModel> Toys { get; set; }
    }
}
