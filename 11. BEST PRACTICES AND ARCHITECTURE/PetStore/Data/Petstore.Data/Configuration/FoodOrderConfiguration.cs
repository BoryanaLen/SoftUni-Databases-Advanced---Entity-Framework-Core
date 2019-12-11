namespace Petstore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class FoodOrderConfiguration : IEntityTypeConfiguration<FoodOrder>
    {
        public void Configure(EntityTypeBuilder<FoodOrder> foodorder)
        {
            foodorder
                .HasKey(k => new { k.FoodId, k.OrderId });

            foodorder
                .HasOne(fo => fo.Food)
                .WithMany(f => f.Orders)
                .HasForeignKey(o => o.FoodId)
                .OnDelete(DeleteBehavior.Restrict);

            foodorder
                .HasOne(fo => fo.Order)
                .WithMany(o => o.Food)
                .HasForeignKey(f => f.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
