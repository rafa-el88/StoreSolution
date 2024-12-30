using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreSolution.Core.Services.Store.Interfaces;
using StoreSolution.Server.Autorization;
using StoreSolution.Server.ViewModels.Store;

namespace StoreSolution.Server.Controllers
{
    [Route("api/movie")]
    [Authorize]
    public class MovieController : Controller
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseApiController> _logger;
        protected readonly IMovieService _movieService;

        public MovieController(IMapper mapper, ILogger<BaseApiController> logger, IMovieService movieService)
        {
            _mapper = mapper;
            _logger = logger;
            _movieService = movieService;
        }

        [HttpGet("movies/{pageNumber}/{pageSize}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<MovieViewModel>))]
        public async Task<IActionResult> GetMovies(int pageNumber, int pageSize)
        {
            var result = await _movieService.GetAllMovies(pageNumber, pageSize);
            var movieViewModel = _mapper.Map<List<MovieViewModel>>(result);
            
            return Ok(movieViewModel);
        }

        [HttpGet("movies")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<MovieViewModel>))]
        public async Task<IActionResult> GetMovies()
        {
            return await GetMovies(-1, -1);
        }
    }
}
