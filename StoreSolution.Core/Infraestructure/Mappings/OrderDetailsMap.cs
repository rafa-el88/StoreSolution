using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Models.Store;

namespace StoreSolution.Core.Infraestructure.Mappings
{
    public class OrderDetailsMap : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("AppOrderDetails");

            builder.Property(p => p.PricePerDay).HasColumnType("numeric(10, 2)");
            builder.Property(p => p.Quantity);
        }
    }
}
