using StoreSolution.Core.Models.Store;

namespace StoreSolution.Core.Services.Store.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomers(int pageNumber, int pageSize);
    }
}
