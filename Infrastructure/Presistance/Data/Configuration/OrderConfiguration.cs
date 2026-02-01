using Domain.Entities.Order_Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, address => address.WithOwner());
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.PaymentStatus).HasConversion(
                s => s.ToString(), // in DB as string
                s => Enum.Parse<OrderPaymentStatus>(s) // return it as Enum value 
                );
            builder.HasOne(o => o.DeliveryMethods).WithMany()
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,3");

               
        }
    }
}
