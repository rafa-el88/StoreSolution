using Newtonsoft.Json;

namespace StoreSolution.Server.ViewModels.Account
{
    public class UserPatchViewModel
    {
        //[JsonProperty("fullName")]
        public string? FullName { get; set; }

        //[JsonProperty("jobTitle")]
        public string? JobTitle { get; set; }

        //[JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        //[JsonProperty("configuration")]
        public string? Configuration { get; set; }
    }
}
