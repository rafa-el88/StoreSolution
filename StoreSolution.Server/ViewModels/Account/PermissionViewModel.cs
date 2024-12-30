using Newtonsoft.Json;
using StoreSolution.Core.Models.Account;
using System.Diagnostics.CodeAnalysis;

namespace StoreSolution.Server.ViewModels.Account
{
    public class PermissionViewModel
    {
        //[JsonProperty("namne")]
        public string? Name { get; set; }

        //[JsonProperty("value")]
        public string? Value { get; set; }

        //[JsonProperty("groupName")]
        public string? GroupName { get; set; }

        //[JsonProperty("description")]
        public string? Description { get; set; }


        [return: NotNullIfNotNull(nameof(permission))]
        public static explicit operator PermissionViewModel?(ApplicationPermission? permission)
        {
            if (permission == null)
                return null;

            return new PermissionViewModel
            {
                Name = permission.Name,
                Value = permission.Value,
                GroupName = permission.GroupName,
                Description = permission.Description
            };
        }
    }
}
