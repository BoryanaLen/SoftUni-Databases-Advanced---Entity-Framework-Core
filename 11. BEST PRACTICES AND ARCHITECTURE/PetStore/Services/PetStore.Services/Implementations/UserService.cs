using Petstore.Data;
using Petstore.Data.Models;
using System.Linq;

namespace PetStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly PetstoreDbContext data;

        public UserService(PetstoreDbContext data)
        {
            this.data = data;
        }

        public bool Exists(int userId)
        {
            return this.data.Users
                .Any(u => u.Id == userId);
        }

        public void Register(string name, string email)
        {
            var user = new User()
            {
                Name = name,
                Email = email
            };

            this.data.Users.Add(user);
            this.data.SaveChanges();
        }
    }
}
