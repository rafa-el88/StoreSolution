using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using StoreSolution.Core.Models.Interface;

namespace StoreSolution.Core.Models.Account
{
    public class ApplicationRole : IdentityRole, IAuditableEntity
    {
        #region Constructors
        public ApplicationRole() { }

        public ApplicationRole(string roleName) : base(roleName) { }

        public ApplicationRole(string roleName, string description) : base(roleName) => Description = description;
        #endregion

        //[JsonProperty("description")]
        public string? Description { get; set; }

        //[JsonProperty("createdBy")]
        public string? CreatedBy { get; set; }

        //[JsonProperty("updatedBy")]
        public string? UpdatedBy { get; set; }

        //[JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        //[JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }

        //[JsonProperty("users")]
        public ICollection<IdentityUserRole<string>> Users { get; } = [];

        //[JsonProperty("claims")]
        public ICollection<IdentityRoleClaim<string>> Claims { get; } = [];
    }
}
