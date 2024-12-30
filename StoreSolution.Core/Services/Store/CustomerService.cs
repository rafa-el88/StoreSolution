using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Infraestructure.Context;
using StoreSolution.Core.Models.Store;
using StoreSolution.Core.Services.Store.Interfaces;

namespace StoreSolution.Core.Services.Store
{
    public class CustomerService : ICustomerService
    {
        private readonly StoreSolutionDbContext _context;

        public CustomerService(StoreSolutionDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomers(int pageNumber, int pageSize)
        {
            IQueryable<Customer> query = _context.Customers.OrderBy(u => u.Name);

            if (pageNumber != -1)
                query = query.Skip((pageNumber - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            var customers = await query.ToListAsync();
            return customers;
        }

        public IEnumerable<Customer> GetTopActiveCustomers(int count)
        {
            throw new NotImplementedException();
        }
    }
}
