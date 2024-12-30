using StoreSolution.Core.Models.Base;

namespace StoreSolution.Core.Models.Store
{
    public class OrderDetail : BaseEntity
    {
        public decimal PricePerDay { get; set; }
        
        public int Quantity { get; set; }
        
        public int MovieId { get; set; }
        
        public required Movie Movie { get; set; }

        public int OrderId { get; set; }
        
        public required Order Order { get; set; }
    }
}
