using FluentValidation;
using Newtonsoft.Json;

namespace StoreSolution.Server.ViewModels.Store
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Gender { get; set; }

        public ICollection<OrderViewModel>? Orders { get; set; }
    }

    public class CustomerViewModelValidator : AbstractValidator<CustomerViewModel>
    {
        public CustomerViewModelValidator()
        {
            RuleFor(register => register.Name).NotEmpty().WithMessage("O nome do cliente não pode estar vazio.");
            RuleFor(register => register.Gender).NotEmpty().WithMessage("O gênero não pode ser vazio.");
        }
    }
}
