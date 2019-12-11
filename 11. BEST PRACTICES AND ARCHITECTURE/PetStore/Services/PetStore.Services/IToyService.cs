using PetStore.Services.Models.Toy;

namespace PetStore.Services
{
    public interface IToyService
    {
        void BuyFromDistriburor(string name, string description, decimal distributorPrice,
            double profit, int brandId, int categoryId);

        void BuyFromDistriburor(AddingToyServiceModel model);

        void SellToyToUser(int toyId, int userId);

        public bool Exists(int toyId);

    }
}
