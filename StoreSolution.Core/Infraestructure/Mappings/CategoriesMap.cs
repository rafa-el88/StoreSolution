using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Models.Store;

namespace StoreSolution.Core.Infraestructure.Mappings
{
    public class CategoriesMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("AppCategories");

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p=> p.IsActive).HasDefaultValue(true);
        }
    }
}
