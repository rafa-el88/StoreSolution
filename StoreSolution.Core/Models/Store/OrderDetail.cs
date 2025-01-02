using StoreSolution.Core.Models.Base;

namespace StoreSolution.Core.Models.Store
{
    public class OrderDetail : BaseEntity
    {
        public decimal PricePerDay { get; set; }
        
        public int MovieId { get; set; }
        
        public  int OrderId { get; set; }

        public virtual Movie? Movie { get; set; }
        
        public virtual Order? Order { get; set; }
    }
}
