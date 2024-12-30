using StoreSolution.Core.Models.Base;

namespace StoreSolution.Core.Models.Store
{
    public class Movie : BaseEntity
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public string? Sinopse { get; set; }

        public decimal PricePerDay { get; set; }

        public int UnitsInStock { get; set; }

        public bool IsActive { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public string? UrlHandle { get; set; }

        public int? ParentId { get; set; }
        public Movie? Parent { get; set; }

        public int MovieCategoryId { get; set; }

        public required Category MovieCategory { get; set; }

        public ICollection<Movie> Children { get; } = [];

        public ICollection<OrderDetail> OrderDetails { get; } = [];
    }
}
