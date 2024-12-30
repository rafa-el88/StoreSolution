using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreSolution.Core.Services.Store.Interfaces;
using StoreSolution.Server.Autorization;
using StoreSolution.Server.ViewModels.Store;

namespace StoreSolution.Server.Controllers
{
    [Route("api/customer")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(IMapper mapper, ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<CustomerViewModel>))]
        public async Task<IActionResult> GetCustomers(int pageNumber, int pageSize)
        {
            var result = await _customerService.GetCustomers(pageNumber, pageSize);
            var customerViewModel = _mapper.Map<List<CustomerViewModel>>(result);

            return Ok(customerViewModel);
        }

        [HttpGet()]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<CustomerViewModel>))]
        public async Task<IActionResult> GetAllCustomers()
        {
            return await GetCustomers(-1, -1);
        }
    }
}
