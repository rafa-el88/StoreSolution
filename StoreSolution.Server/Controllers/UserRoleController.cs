using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreSolution.Core.Models.Account;
using StoreSolution.Core.Services.Account.Exceptions;
using StoreSolution.Core.Services.Account.Interfaces;
using StoreSolution.Core.Services.Account;
using StoreSolution.Server.Autorization;
using StoreSolution.Server.ViewModels.Account;

namespace StoreSolution.Server.Controllers
{
    [Route("api/account")]
    [Authorize]
    public class UserRoleController : BaseApiController
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IAuthorizationService _authorizationService;

        public UserRoleController(ILogger<UserRoleController> logger, IMapper mapper,
            IUserRoleService userRoleService, IAuthorizationService authorizationService) : base(logger, mapper)
        {
            _userRoleService = userRoleService;
            _authorizationService = authorizationService;
        }

        [HttpGet("roles/{id}", Name = nameof(GetRoleById))]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var appRole = await _userRoleService.GetRoleByIdAsync(id);

            if (!(await _authorizationService.AuthorizeAsync(User, appRole?.Name ?? string.Empty,
                AuthPolicies.ViewRoleByRoleNamePolicy)).Succeeded)
                return new ChallengeResult();

            var roleViewModel = appRole != null ? await GetRoleViewModelHelper(appRole.Name!) : null;

            if (roleViewModel != null)
                return Ok(roleViewModel);

            return NotFound(id);
        }

        [HttpGet("roles/name/{name}")]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleByName(string name)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, name,
                AuthPolicies.ViewRoleByRoleNamePolicy)).Succeeded)
                return new ChallengeResult();

            var roleViewModel = await GetRoleViewModelHelper(name);

            if (roleViewModel != null)
                return Ok(roleViewModel);

            return NotFound(name);
        }

        [HttpGet("roles")]
        [Authorize(AuthPolicies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles()
        {
            return await GetRoles(-1, -1);
        }

        [HttpGet("roles/{pageNumber:int}/{pageSize:int}")]
        [Authorize(AuthPolicies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles(int pageNumber, int pageSize)
        {
            var roles = await _userRoleService.GetRolesLoadRelatedAsync(pageNumber, pageSize);
            return Ok(_mapper.Map<List<RoleViewModel>>(roles));
        }

        [HttpPut("roles/{id}")]
        [Authorize(AuthPolicies.ManageAllRolesPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleViewModel role)
        {
            if (role == null)
                return BadRequest($"{nameof(role)} cannot be null");

            var appRole = await _userRoleService.GetRoleByIdAsync(id);

            if (appRole == null)
                return NotFound(id);

            if (!string.IsNullOrWhiteSpace(role.Id) && id != role.Id)
                return BadRequest("Conflicting role id in parameter and model data");

            _mapper.Map(role, appRole);

            var result = await _userRoleService
                .UpdateRoleAsync(appRole, role.Permissions?.Select(p => p.Value!).ToArray());

            if (result.Succeeded)
                return NoContent();

            AddModelError(result.Errors);

            return BadRequest(ModelState);
        }

        [HttpPost("roles")]
        [Authorize(AuthPolicies.ManageAllRolesPolicy)]
        [ProducesResponseType(201, Type = typeof(RoleViewModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRole([FromBody] RoleViewModel role)
        {
            if (role == null)
                return BadRequest($"{nameof(role)} cannot be null");

            var appRole = _mapper.Map<ApplicationRole>(role);

            var result = await _userRoleService
                .CreateRoleAsync(appRole, role.Permissions?.Select(p => p.Value!).ToArray() ?? []);

            if (result.Succeeded)
            {
                var roleViewModel = await GetRoleViewModelHelper(appRole.Name!);
                return CreatedAtAction(nameof(GetRoleById), new { id = roleViewModel?.Id }, roleViewModel);
            }

            AddModelError(result.Errors);

            return BadRequest(ModelState);
        }

        [HttpDelete("roles/{id}")]
        [Authorize(AuthPolicies.ManageAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var appRole = await _userRoleService.GetRoleByIdAsync(id);

            if (appRole == null)
                return NotFound(id);

            var canDelete = await _userRoleService.TestCanDeleteRoleAsync(id);
            if (!canDelete.Success)
            {
                AddModelError($"Role \"{appRole.Name}\" cannot be deleted at this time. " +
                    "Delete the associated records and try again");
                AddModelError(canDelete.Errors, "Records");
            }

            if (ModelState.IsValid)
            {
                var roleVM = await GetRoleViewModelHelper(appRole.Name!, false);
                var result = await _userRoleService.DeleteRoleAsync(appRole);

                if (!result.Succeeded)
                {
                    throw new UserRoleException($"The following errors occurred whilst deleting role \"{id}\": " +
                        $"{string.Join(", ", result.Errors)}");
                }
                return Ok(roleVM);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("permissions")]
        [Authorize(AuthPolicies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<PermissionViewModel>))]
        public IActionResult GetAllPermissions()
        {
            return Ok(_mapper.Map<List<PermissionViewModel>>(ApplicationPermissions.AllPermissions));
        }

        private async Task<RoleViewModel?> GetRoleViewModelHelper(string roleName, bool loadRelatedEntities = true)
        {
            var role = loadRelatedEntities ? await _userRoleService.GetRoleLoadRelatedAsync(roleName)
                : await _userRoleService.GetRoleByNameAsync(roleName);

            if (role != null)
                return _mapper.Map<RoleViewModel>(role);

            return null;
        }
    }
}