using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Ria.Services.Customer.Web.Controllers;

[ApiController]
[Route("/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<Customer> _customerValidator;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ICustomerRepository customerRepository, IValidator<Customer> customerValidator, ILogger<CustomerController> logger)
    {
        _customerRepository = customerRepository;
        _customerValidator = customerValidator;
        _logger = logger;
    }

    [HttpGet]
    public Customer[] GetCustomers()
    {
        _logger.LogInformation($"{_customerRepository.Customers.Length} customers retrieved.");
        return _customerRepository.Customers;   
    }
    
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
        
        await _customerRepository.AddCustomers(customers.ToArray());
        
        _logger.LogInformation($"Inserted {customers.Length} new customers. Total is {_customerRepository.Customers.Length}");

        return Ok();
    }
}