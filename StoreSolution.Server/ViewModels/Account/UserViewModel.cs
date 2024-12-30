using Newtonsoft.Json;
using StoreSolution.Server.Atributes;
using StoreSolution.Server.ViewModels.Account.Base;

namespace StoreSolution.Server.ViewModels.Account
{
    public class UserViewModel : UserBaseViewModel
    {
        //[JsonProperty("isLockedOut")]
        public bool IsLockedOut { get; set; }

        //[JsonProperty("roles")]
        [MinimumCount(1, ErrorMessage = "Roles cannot be empty")]
        public string[]? Roles { get; set; }
    }
}
