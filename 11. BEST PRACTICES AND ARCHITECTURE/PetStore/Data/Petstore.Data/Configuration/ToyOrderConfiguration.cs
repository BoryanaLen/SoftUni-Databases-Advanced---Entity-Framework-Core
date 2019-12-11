namespace Petstore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    class ToyOrderConfiguration : IEntityTypeConfiguration<ToyOrder>
    {
        public void Configure(EntityTypeBuilder<ToyOrder> toyorder)
        {
            toyorder
                .HasKey(k => new { k.ToyId, k.OrderId });

            toyorder
                .HasOne(to => to.Toy)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.ToyId)
                .OnDelete(DeleteBehavior.Restrict);

            toyorder
               .HasOne(to => to.Order)
               .WithMany(o => o.Toys)
               .HasForeignKey(t => t.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
