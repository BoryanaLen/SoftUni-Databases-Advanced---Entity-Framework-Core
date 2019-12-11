using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petstore.Data.Models;

namespace Petstore.Data.Configuration
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> food)
        {
            food.HasKey(f => f.Id);
        }
    }
}
