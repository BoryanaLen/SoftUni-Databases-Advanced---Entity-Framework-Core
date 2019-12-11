using Petstore.Data;
using Petstore.Data.Models;

namespace PetStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly PetstoreDbContext data;

        public OrderService(PetstoreDbContext data)
        {
            this.data = data;
        }

        public void CompleteOrder(int orderId)
        {
            var order = this.data
                 .Orders
                 .Find(orderId);

            order.Status = OrderStatus.Done;

            this.data.SaveChanges();
        }
    }
}
