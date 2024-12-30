using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Models.Store;

namespace StoreSolution.Core.Infraestructure.Mappings
{
    public class MoviesMap : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("AppMovies");

            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.HasIndex(p => p.Title);
            builder.Property(p => p.Description).HasMaxLength(100);
            builder.Property(p => p.Sinopse).HasMaxLength(1000);
            builder.Property(p => p.PricePerDay).HasColumnType("numeric(10, 2)");
            builder.Property(p => p.UnitsInStock);
            builder.Property(p => p.IsActive).HasDefaultValue(true);
            builder.Property(p => p.FeaturedImageUrl).HasMaxLength(255);
            builder.Property(p => p.UrlHandle).HasMaxLength(255);

            builder.HasOne(p => p.Parent)
                .WithMany(p => p.Children)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
