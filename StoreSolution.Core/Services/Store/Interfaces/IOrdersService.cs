using StoreSolution.Core.Models.Store;

namespace StoreSolution.Core.Services.Store.Interfaces
{
    public interface IOrdersService
    {
        Task<List<Order>> GetAllOrders(int pageNumber, int pageSize);
    }
}
