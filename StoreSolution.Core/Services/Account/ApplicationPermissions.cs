using StoreSolution.Core.Models.Account;
using System.Collections.ObjectModel;

namespace StoreSolution.Core.Services.Account
{
    public static class ApplicationPermissions
    {
        //USER PERMISSIONS
        public const string UsersPermissionGroupName = "User Permissions";

        public static readonly ApplicationPermission ViewUsers = new(
            "View Users",
            "users.view",
            UsersPermissionGroupName,
            "Permission to view other users account details");

        public static readonly ApplicationPermission ManageUsers = new(
            "Manage Users",
            "users.manage",
            UsersPermissionGroupName,
            "Permission to create, delete and modify other users account details");

        //ROLE PERMISSIONS
        public const string RolesPermissionGroupName = "Role Permissions";

        public static readonly ApplicationPermission ViewRoles = new(
            "View Roles",
            "roles.view",
            RolesPermissionGroupName,
            "Permission to view available roles");

        public static readonly ApplicationPermission ManageRoles = new(
            "Manage Roles",
            "roles.manage",
            RolesPermissionGroupName,
            "Permission to create, delete and modify roles");

        public static readonly ApplicationPermission AssignRoles = new(
            "Assign Roles",
            "roles.assign",
            RolesPermissionGroupName,
            "Permission to assign roles to users");

        //ALL PERMISSIONS
        public static readonly ReadOnlyCollection<ApplicationPermission> AllPermissions =
            new List<ApplicationPermission> {
                ViewUsers, ManageUsers,
                ViewRoles, ManageRoles, AssignRoles
            }.AsReadOnly();

        //HELPER METHODS
        public static ApplicationPermission? GetPermissionByName(string? permissionName) => 
                    AllPermissions.SingleOrDefault(p => p.Name == permissionName);

        public static ApplicationPermission? GetPermissionByValue(string? permissionValue) =>
                    AllPermissions.SingleOrDefault(p => p.Value == permissionValue);

        public static string[] GetAllPermissionValues() =>
                    AllPermissions.Select(p => p.Value).ToArray();

        public static string[] GetAdministrativePermissionValues() =>
                    new[] { ManageUsers, ManageRoles, AssignRoles }.Select(p => p.Value).ToArray();
    }
}
