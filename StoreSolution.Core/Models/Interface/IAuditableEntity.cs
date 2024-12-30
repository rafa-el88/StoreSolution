using Newtonsoft.Json;

namespace StoreSolution.Core.Models.Interface
{
    public interface IAuditableEntity
    {
        //[JsonProperty("createdBy")]
        string? CreatedBy { get; set; }

        //[JsonProperty("updatedBy")]
        string? UpdatedBy { get; set; }

        //[JsonProperty("createdDate")]
        DateTime CreatedDate { get; set; }

        //[JsonProperty("updatedDate")]
        DateTime UpdatedDate { get; set; }
    }
}
