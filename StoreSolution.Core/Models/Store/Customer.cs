using StoreSolution.Core.Enums;
using StoreSolution.Core.Models.Base;

namespace StoreSolution.Core.Models.Store
{
    public class Customer : BaseEntity
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public Gender? Gender { get; set; }

        public ICollection<Order> Orders { get; } = [];
    }
}
