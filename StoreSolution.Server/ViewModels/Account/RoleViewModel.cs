using Newtonsoft.Json;
using StoreSolution.Core.Extensions;
using StoreSolution.Server.Atributes;
using System.ComponentModel.DataAnnotations;

namespace StoreSolution.Server.ViewModels.Account
{
    public class RoleViewModel : ISanitizeModel
    {
        public virtual void SanitizeModel()
        {
            Id = Id.NullIfWhiteSpace();
            Name = Name.NullIfWhiteSpace();
            Description = Description.NullIfWhiteSpace();
        }

        //[JsonProperty("id")]
        public string? Id { get; set; }

        //[JsonProperty("name")]
        [Required(ErrorMessage = "Role name is required"),
         StringLength(200, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 200 characters")]
        public string? Name { get; set; }

        //[JsonProperty("description")]
        public string? Description { get; set; }

        //[JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        //[JsonProperty("permissions")]
        public PermissionViewModel[]? Permissions { get; set; }

    }
}
