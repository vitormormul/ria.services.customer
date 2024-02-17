using Microsoft.AspNetCore.Mvc;

namespace Ria.Services.Customer.Web.Controllers;

[ApiController]
[Route("/customers")]
public class CustomerController
{
    private readonly CustomerRepository _customerRepository;

    public CustomerController(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public Customer[] GetCustomers() => _customerRepository.Customers;
    
    [HttpPost]
    public void AddCustomers(IEnumerable<Customer> customers)
    {
        _customerRepository.AddCustomers(customers.ToArray());
    }
}