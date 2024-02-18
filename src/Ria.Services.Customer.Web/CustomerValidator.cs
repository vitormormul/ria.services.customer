using FluentValidation;

namespace Ria.Services.Customer.Web;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator(ICustomerRepository customerRepository)
    {
        RuleFor(customer => customer.Id)
            .GreaterThan(-1)
            .Must(id => !customerRepository.IdExists(id))
            .WithMessage(customer => $"Id {customer.Id} already exists.");
        RuleFor(customer => customer.Age)
            .NotNull()
            .NotEmpty()
            .GreaterThan(18);
        RuleFor(customer => customer.LastName)
            .NotNull()
            .NotEmpty();
        RuleFor(customer => customer.FirstName)
            .NotNull()
            .NotEmpty();
    }
}