using StoreSolution.Core.Models.Store;

namespace StoreSolution.Core.Services.Store.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllMovies(int pageNumber, int pageSize);
    }
}
