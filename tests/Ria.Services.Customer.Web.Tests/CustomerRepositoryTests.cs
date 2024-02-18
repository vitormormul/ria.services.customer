using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Ria.Services.Customer.Web.Tests;

public class CustomerRepositoryTests
{
    private readonly CustomerRepository _customerRepository;
    private readonly Mock<ICustomerPublisher> _customerPublisherMock;
    
    public CustomerRepositoryTests()
    {
        var autoMocker = new AutoMocker();

        _customerPublisherMock = autoMocker.GetMock<ICustomerPublisher>();
        _customerRepository = autoMocker.CreateInstance<CustomerRepository>();
    }

    [Fact]
    public async Task AddCustomers_ShouldNotAllowSameId_WhenCalled()
    {
        // Arrange
        var customers = new[] { new Customer { Id = 1 } };
        
        // Act
        await _customerRepository.AddCustomers(customers);
        async Task AddCustomer() => await _customerRepository.AddCustomers(customers);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(AddCustomer);
        _customerPublisherMock.Verify(x => x.WriteAsync(It.IsAny<Customer[]>()), Times.Once);
    }

    [Fact]
    public async Task AddCustomers_ShouldAddDifferentCustomersAndSort_WhenCalled()
    {
        // Arrange
        var customers = new[] {
            new Customer { Id = 2, FirstName = "Bob", LastName = "Brown"},
            new Customer { Id = 1, FirstName = "Anna", LastName = "Brown"},
            new Customer { Id = 3, FirstName = "Anna", LastName = "Frank"},
            new Customer { Id = 2, FirstName = "William", LastName = "David"},
            new Customer { Id = 2, FirstName = "Bob", LastName = "David"},
        };

        var payload = new Customer[5];
        customers.CopyTo(payload, 0);
        
        // Act
        await _customerRepository.AddCustomers(payload);
        var response = _customerRepository.Customers;

        // Assert
        Assert.Equal(customers[0], response[1]);
        Assert.Equal(customers[1], response.First());
        Assert.Equal(customers[2], response.Last());
        Assert.Equal(customers[3], response[3]);
        Assert.Equal(customers[4], response[2]);
        _customerPublisherMock.Verify(x => x.WriteAsync(It.IsAny<Customer[]>()), Times.Once);
    }
}