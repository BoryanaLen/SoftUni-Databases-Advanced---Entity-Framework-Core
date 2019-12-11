namespace PetStore.Services.Implementations
{
    using System;
    using System.Linq;
    using Petstore.Data;
    using Petstore.Data.Models;
    using PetStore.Services.Models.Food;


    public class FoodService : IFoodService
    {
        private readonly PetstoreDbContext data;

        private readonly IUserService userService;

        public FoodService(PetstoreDbContext data, IUserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public void BuyFromDistributor(string name, double weight, decimal distributorPrice, double profit,
            DateTime expirationDate, int brandId, int categoryId)
        {
            var brand = this.data.Brands
                    .FirstOrDefault(br => br.Name.ToLower() == name.ToLower());

            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }

            //Profit should be in range 0-500%
            if (profit < 0 || profit > 5)
            {
                throw new ArgumentException("Profit must be higher than zero and lower than 500%!");
            }

            var food = new Food()
            {
               Name = name,
               Weight = weight,
               DestributorPrice = distributorPrice,
               Price = distributorPrice + (distributorPrice * (decimal)profit),
               EnspirationDate = expirationDate,
               BrandId = brandId,
               CategoryId = categoryId
            };

            this.data.Foods.Add(food);
            this.data.SaveChanges();
        }

        public void BuyFromDistributor(AddingFoodServiceModel model)
        {
            if (String.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }

            if (model.Profit < 0 || model.Profit > 5)
            {
                throw new ArgumentException("Profit must be higher than zero and lower than 500%!");
            }

            var food = new Food()
            {
                Name = model.Name,
                Weight = model.Weight,
                DestributorPrice = model.Price,
                Price = model.Price + (model.Price * (decimal)model.Profit),
                EnspirationDate = model.ExpirationDate,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

            this.data.Foods.Add(food);
            this.data.SaveChanges();
        }

        public bool Exists(int foodId)
        {
            return this.data.Foods.Any(f => f.Id == foodId);
        }

        public void SellFoodToUser(int foodId, int userId)
        {
            if (!this.Exists(foodId))
            {
                throw new ArgumentException("There is no such food with given id in the database!");
            }

            if (!this.userService.Exists(userId))
            {
                throw new ArgumentException("There is no such user with given id in the database!");
            }

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = OrderStatus.Done,
                UserId = userId
            };

            var foodOrder = new FoodOrder()
            {
                FoodId = foodId,
                Order = order
            };

            this.data.Orders.Add(order);
            this.data.FoodOrders.Add(foodOrder);

            data.SaveChanges();
        }
    }
}
