using StoreSolution.Core.Extensions;
using StoreSolution.Server.Atributes;
using System.ComponentModel.DataAnnotations;

namespace StoreSolution.Server.ViewModels.Account.Base
{
    public abstract class UserBaseViewModel : ISanitizeModel
    {
        public virtual void SanitizeModel()
        {
            Id = Id.NullIfWhiteSpace();
            FullName = FullName.NullIfWhiteSpace();
            JobTitle = JobTitle.NullIfWhiteSpace();
            PhoneNumber = PhoneNumber.NullIfWhiteSpace();
            Configuration = Configuration.NullIfWhiteSpace();
        }

        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required"),
         StringLength(200, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 200 characters")]
        public required string UserName { get; set; }

        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email is required"),
         StringLength(200, ErrorMessage = "Email must be at most 200 characters"),
         EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        public string? JobTitle { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Configuration { get; set; }

        public bool IsEnabled { get; set; }
    }
}
