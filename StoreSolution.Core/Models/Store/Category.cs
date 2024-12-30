using StoreSolution.Core.Models.Base;

namespace StoreSolution.Core.Models.Store
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Movie> Movies { get; } = [];
    }
}
