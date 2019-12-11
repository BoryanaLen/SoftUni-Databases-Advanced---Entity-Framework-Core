using Microsoft.EntityFrameworkCore;
using Petstore.Data.Models;

namespace Petstore.Data
{
    public class PetstoreDbContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Toy> Toys { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FoodOrder> FoodOrders { get; set; }
        public DbSet<ToyOrder> ToyOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(DataSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
           =>  builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);           

    }
}
