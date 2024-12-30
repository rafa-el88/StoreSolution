using Newtonsoft.Json;

namespace StoreSolution.Server.ViewModels.Account
{
    public class ClaimViewModel
    {
        //[JsonProperty("type")]
        public string? Type { get; set; }

        //[JsonProperty("value")]
        public string? Value { get; set; }
    }
}
