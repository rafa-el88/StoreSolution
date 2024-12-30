using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Models.Store;

namespace StoreSolution.Core.Infraestructure.Mappings
{
    public class OrdersMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("AppOrders");


            builder.Property(o => o.DateStartRental).IsRequired();
            builder.Property(o => o.DateEndRental).IsRequired();
            builder.Property(o => o.DateDevolution).IsRequired(false);
            builder.Property(o => o.ReturnedMovie).HasDefaultValue(false);
        }
    }
}