namespace PetStore.Services
{
    using PetStore.Services.Models.Food;
    using System;

    public interface IFoodService
    {
        void BuyFromDistributor(string name, double weight, decimal distributorPrice, double profit,
            DateTime expirationDate, int brandId, int categoryId);

        void BuyFromDistributor(AddingFoodServiceModel model);

        void SellFoodToUser(int foodId, int userId);

        public bool Exists(int foodId);
    }
}
