using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Infraestructure.Context;
using StoreSolution.Core.Models.Store;
using StoreSolution.Core.Services.Store.Interfaces;

namespace StoreSolution.Core.Services.Store
{
    public class OrdersService : IOrdersService
    {
        private readonly StoreSolutionDbContext _context;

        public OrdersService(StoreSolutionDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrders(int pageNumber, int pageSize)
        {
            IQueryable<Order> query = _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Customer)
                .Include(o => o.Cashier)
                .OrderBy(u => u.CreatedDate);

            if (pageNumber != -1)
                query = query.Skip((pageNumber - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync(); 
        }
    }
}
