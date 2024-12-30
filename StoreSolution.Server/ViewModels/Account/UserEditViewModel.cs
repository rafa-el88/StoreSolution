using Newtonsoft.Json;
using StoreSolution.Server.Atributes;
using StoreSolution.Server.ViewModels.Account.Base;
using System.ComponentModel.DataAnnotations;

namespace StoreSolution.Server.ViewModels.Account
{
    public class UserEditViewModel : UserBaseViewModel
    {
        //[JsonProperty("currentPassword")]
        public string? CurrentPassword { get; set; }

        //[JsonProperty("newPassword")]
        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        public string? NewPassword { get; set; }

        //[JsonProperty("roles")]
        [MinimumCount(1, false, ErrorMessage = "Roles cannot be empty")]
        public string[]? Roles { get; set; }
    }
}
