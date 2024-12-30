using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreSolution.Core.Infraestructure;
using StoreSolution.Core.Infraestructure.Context;
using StoreSolution.Core.Models.Account;
using StoreSolution.Core.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StoreSolution.Core.Services.Account
{
    public class UserRoleService : IUserRoleService
    {
        private readonly StoreSolutionDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleService(StoreSolutionDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId) =>
                await _roleManager.FindByIdAsync(roleId);

        public async Task<ApplicationRole?> GetRoleByNameAsync(string roleName) =>
                await _roleManager.FindByNameAsync(roleName);

        public async Task<ApplicationRole?> GetRoleLoadRelatedAsync(string roleName) =>
                await _context.Roles
                    .Include(r => r.Claims)
                    .Include(r => r.Users)
                    .AsSingleQuery()
                    .Where(r => r.Name == roleName)
                    .SingleOrDefaultAsync();

        public async Task<List<ApplicationRole>> GetRolesLoadRelatedAsync(int page, int pageSize)
        {
            IQueryable<ApplicationRole> rolesQuery = _context.Roles
                .Include(r => r.Claims)
                .Include(r => r.Users)
                .AsSingleQuery()
                .OrderBy(r => r.Name);

            if (page != -1)
                rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            return await rolesQuery.ToListAsync();
        }

        public async Task<(bool Succeeded, string[] Errors)> CreateRoleAsync(ApplicationRole role, IEnumerable<string> claims)
        {
            var invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();

            if (invalidClaims.Length != 0)
                return (false, new[] { $"The following claim types are invalid: {string.Join(", ", invalidClaims)}" });

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            role = (await _roleManager.FindByNameAsync(role.Name!))!;

            foreach (var claim in claims.Distinct())
            {
                result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaims.Permission, ApplicationPermissions.GetPermissionByValue(claim)!));

                if (!result.Succeeded)
                {
                    await DeleteRoleAsync(role);
                    return (false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return (true, []);
        }

        public async Task<(bool Succeeded, string[] Errors)> UpdateRoleAsync(ApplicationRole role, IEnumerable<string>? claims)
        {
            if (claims != null)
            {
                var invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();

                if (invalidClaims.Length != 0)
                    return (false, new[] { $"The following claim types are invalid: {string.Join(", ", invalidClaims)}" });
            }

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            if (claims != null)
            {
                var roleClaims = (await _roleManager.GetClaimsAsync(role)).Where(c => c.Type == CustomClaims.Permission);
                var roleClaimValues = roleClaims.Select(c => c.Value).ToArray();
                var claimsToRemove = roleClaimValues.Except(claims).ToArray();
                var claimsToAdd = claims.Except(roleClaimValues).Distinct().ToArray();

                if (claimsToRemove.Length != 0)
                {
                    foreach (var claim in claimsToRemove)
                    {
                        result = await _roleManager.RemoveClaimAsync(role, roleClaims.Where(c => c.Value == claim).First());

                        if (!result.Succeeded)
                            return (false, result.Errors.Select(e => e.Description).ToArray());
                    }
                }

                if (claimsToAdd.Length != 0)
                {
                    foreach (var claim in claimsToAdd)
                    {
                        result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaims.Permission, ApplicationPermissions.GetPermissionByValue(claim)!));

                        if (!result.Succeeded)
                            return (false, result.Errors.Select(e => e.Description).ToArray());
                    }
                }
            }

            return (true, []);
        }

        public async Task<(bool Success, string[] Errors)> TestCanDeleteRoleAsync(string roleId)
        {
            var errors = new List<string>();

            if (await _context.UserRoles.Where(r => r.RoleId == roleId).AnyAsync())
                errors.Add("Role has associated users");

            return (errors.Count == 0, errors.ToArray());
        }

        public async Task<(bool Succeeded, string[] Errors)> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
                return await DeleteRoleAsync(role);

            return (true, []);
        }

        public async Task<(bool Succeeded, string[] Errors)> DeleteRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }
    }
}
