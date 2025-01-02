using StoreSolution.Core.Models.Base;

namespace StoreSolution.Core.Models.Store
{
    public class Movie : BaseEntity
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public string? Sinopse { get; set; }

        public decimal PricePerDay { get; set; }

        public int QuantityCopies { get; set; }

        public int UnitsInStock { get; set; }

        public bool IsActive { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public string? UrlHandle { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; } = [];

        //public int? ParentId { get; set; 
        //public virtual Movie? Parent { get; set; }
        //public virtual ICollection<Movie> Children { get; } = [];
    }
}
