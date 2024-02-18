using System.Linq;
using Xunit;

namespace Ria.Services.Customer.Web.Tests;

public class CustomerUtilitiesTests
{
    [Fact]
    public void Sort_ShouldSortCustomersAlphabetically_WhenCalled()
    {
        // Arrange
        var a = new Customer { FirstName = "Anna", LastName = "Zurich" };
        var b = new Customer { FirstName = "Frank", LastName = "Zurich" };
        var c = new Customer { FirstName = "Frank", LastName = "Anthony" };

        var customers = new[] { a, b, c };

        // Act
        customers.Sort();

        // Assert
        Assert.Equal(a, customers[1]);
        Assert.Equal(b, customers.Last());
        Assert.Equal(c, customers.First());
    }
}