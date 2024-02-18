using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Ria.Services.Customer.Web.Controllers;

[ApiController]
[Route("/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerRepository _customerRepository;
    private readonly IValidator<Customer> _customerValidator;

    public CustomerController(CustomerRepository customerRepository, IValidator<Customer> customerValidator)
    {
        _customerRepository = customerRepository;
        _customerValidator = customerValidator;
    }

    [HttpGet]
    public Customer[] GetCustomers() => _customerRepository.Customers;
    
    [HttpPost]
    public async Task<IActionResult> AddCustomers([FromBody] Customer[] customers)
    {
        var errors = new List<string>();
        Parallel.ForEach(customers, async customer =>
        {
            var validationResult = await _customerValidator.ValidateAsync(customer);
            if (!validationResult.IsValid)
                errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
        });

        if (errors.Any())
            return BadRequest(errors.Distinct());
        
        _customerRepository.AddCustomers(customers.ToArray());

        return Ok();
    }
}