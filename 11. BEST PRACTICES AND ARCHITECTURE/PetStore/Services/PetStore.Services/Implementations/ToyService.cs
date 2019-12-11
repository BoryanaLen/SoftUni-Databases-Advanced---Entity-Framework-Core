using Petstore.Data;
using Petstore.Data.Models;
using PetStore.Services.Models.Toy;
using System;
using System.Linq;

namespace PetStore.Services.Implementations
{
    public class ToyService : IToyService
    {
        private readonly PetstoreDbContext data;

        private readonly IUserService userService;

        public ToyService(PetstoreDbContext data, IUserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public void BuyFromDistriburor(string name, string description, decimal distributorPrice, 
            double profit, int brandId, int categoryId)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }

            //Profit should be in range 0-500%
            if (profit < 0 || profit > 5)
            {
                throw new ArgumentException("Profit must be higher than zero and lower than 500%!");
            }

            var toy = new Toy
            {
                Name = name,
                Description = description,
                DestributorPrice = distributorPrice,
                Price = distributorPrice + distributorPrice * (decimal)profit,
                Profit = profit,
                BrandId = brandId,
                CategoryId = categoryId
            };

            this.data.Toys.Add(toy);

            data.SaveChanges();
        }

        public void BuyFromDistriburor(AddingToyServiceModel model)
        {
            if (String.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }

            //Profit should be in range 0-500%
            if (model.Profit < 0 || model.Profit > 5)
            {
                throw new ArgumentException("Profit must be higher than zero and lower than 500%!");
            }

            var toy = new Toy()
            {
                Name = model.Name,
                Description = model.Description,
                DestributorPrice = model.Price,
                Price = model.Price + model.Price * (decimal)model.Profit,
                Profit = model.Profit,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

            this.data.Toys.Add(toy);

            data.SaveChanges();
        }

        public bool Exists(int toyId)
        {
            return this.data.Toys.Any(t => t.Id == toyId);
        }

        public void SellToyToUser(int toyId, int userId)
        {
            if (!this.Exists(toyId))
            {
                throw new ArgumentException("There is no such toy with given id in the database!");
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

            var toyOrder = new ToyOrder()
            {
                ToyId = toyId,
                Order = order
            };

            this.data.Orders.Add(order);

            this.data.ToyOrders.Add(toyOrder);

            this.data.SaveChanges();
        }
    }
}
