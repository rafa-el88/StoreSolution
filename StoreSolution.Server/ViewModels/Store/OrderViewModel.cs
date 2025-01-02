using Newtonsoft.Json;
using StoreSolution.Core.Models.Account;
using StoreSolution.Core.Models.Store;

namespace StoreSolution.Server.ViewModels.Store
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime DateStartRental { get; set; }

        public DateTime DateEndRental { get; set; }

        public DateTime? DateDevolution { get; set; }

        public bool ReturnedMovie { get; set; }

        //public ApplicationUser? Cashier { get; set; }

        //public Customer? Customer { get; set; }

        //public virtual ICollection<OrderDetail>? OrderDetails { get; } = [];
    }
}
