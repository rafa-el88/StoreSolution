using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Infraestructure.Context;
using StoreSolution.Core.Models.Store;
using StoreSolution.Core.Services.Store.Interfaces;

namespace StoreSolution.Core.Services.Store
{
    public class MovieService : IMovieService
    {
        private readonly StoreSolutionDbContext _context;

        public MovieService(StoreSolutionDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAllMovies(int pageNumber, int pageSize)
        {
            IQueryable<Movie> query = _context.Movies.OrderBy(u => u.Title);

            if (pageNumber != -1)
                query = query.Skip((pageNumber - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
