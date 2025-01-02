using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreSolution.Core.Services.Store.Interfaces;
using StoreSolution.Server.Autorization;
using StoreSolution.Server.ViewModels.Store;

namespace StoreSolution.Server.Controllers.Store
{
    [Route("api/order")]
    [Authorize]
    public class OrderController : Controller
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseApiController> _logger;
        protected readonly IOrdersService _orderService;

        public OrderController(IMapper mapper, ILogger<BaseApiController> logger, IOrdersService orderService)
        {
            _mapper = mapper;
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<OrderViewModel>))]
        public async Task<IActionResult> GetAllOrders(int pageNumber, int pageSize)
        {
            var result = await _orderService.GetAllOrders(pageNumber, pageSize);
            var orderViewModel = _mapper.Map<List<OrderViewModel>>(result);

            return Ok(orderViewModel);
        }

        [HttpGet()]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<OrderViewModel>))]
        public async Task<IActionResult> GetAllOrders()
        {
            return await GetAllOrders(-1, -1);
        }
    }
}
