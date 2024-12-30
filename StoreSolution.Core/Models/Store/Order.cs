using StoreSolution.Core.Models.Account;
using StoreSolution.Core.Models.Base;

namespace StoreSolution.Core.Models.Store
{
    public class Order : BaseEntity
    {
        public DateTime DateStartRental { get; set; }

        public DateTime DateEndRental { get; set; }

        public DateTime? DateDevolution { get; set; }

        public bool ReturnedMovie { get; set; }

        public string? CashierId { get; set; }

        public ApplicationUser? Cashier { get; set; }

        public int CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; } = [];
    }
}
