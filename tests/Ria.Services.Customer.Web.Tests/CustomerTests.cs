using Xunit;

namespace Ria.Services.Customer.Web.Tests;

public class CustomerTests
{
    [Fact]
    public void ComparisonOperators_ShouldBeSortedAlphabetically_WhenCalled()
    {
        // Arrange
        var a = new Customer { FirstName = "Anna", LastName = "Zurich" };
        var b = new Customer { FirstName = "Frank", LastName = "Zurich" };
        var c = new Customer { FirstName = "Frank", LastName = "Anthony" };

        // Act + Assert
        Assert.True(a < b);
        Assert.True(b > a);
        Assert.True(a > c);
        Assert.True(c < a);
        Assert.True(b > c);
        Assert.True(c < b);
    }
}