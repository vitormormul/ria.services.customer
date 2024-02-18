using System;
using FluentValidation.TestHelper;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Ria.Services.Customer.Web.Tests;

public class CustomerValidatorTests
{
    private readonly CustomerValidator _customerValidator;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;

    public CustomerValidatorTests()
    {
        var autoMocker = new AutoMocker();
        
        _customerRepositoryMock = autoMocker.GetMock<ICustomerRepository>();
        _customerValidator = new CustomerValidator(_customerRepositoryMock.Object);
    }

    [Fact]
    public void Validator_ShouldHaveErrors_WhenPayloadIsNotOk()
    {
        // Arrange
        var customer = new Customer
        {
            Age = 12,
            FirstName = String.Empty,
            LastName = String.Empty,
            Id = 2
        };

        _customerRepositoryMock
            .Setup(x => x.IdExists(It.IsAny<int>()))
            .Returns(true);

        // Act
        var result = _customerValidator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Age);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_ShouldNotHaveErrors_WhenPayloadIsOk()
    {
        // Arrange
        var customer = new Customer
        {
            Age = 22,
            FirstName = "Foo",
            LastName = "Bar",
            Id = 2
        };

        _customerRepositoryMock
            .Setup(x => x.IdExists(It.IsAny<int>()))
            .Returns(false);

        // Act
        var result = _customerValidator.TestValidate(customer);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Age);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}