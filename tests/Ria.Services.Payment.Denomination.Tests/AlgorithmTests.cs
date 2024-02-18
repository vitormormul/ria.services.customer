using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ria.Services.Payment.Denomination.Tests;

public class AlgorithmTests
{
    [Fact]
    public void Program_ShouldReturnCorrectResponse_WhenCalled()
    {
        // Arrange
        const int payment = 210;
        var expected = new List<Dictionary<int, int>>
        {
            new (){{100, 2}, {10, 1}},
            new (){{100, 1}, {50, 2}, {10, 1}},
            new (){{100, 1}, {50, 1}, {10, 6}},
            new (){{100, 1}, {10, 11}},
            new (){{50, 4}, {10, 1}},
            new (){{50, 3}, {10, 6}},
            new (){{50, 2}, {10, 11}},
            new (){{50, 1}, {10, 16}},
            new (){{10, 21}}
        };

        // Act
        Main.Run(payment);

        // Assert
        Assert.True(Main.Denominations.All(x => expected.Any(y => DictionaryComparison(x, y))));
    }

    private bool DictionaryComparison(Dictionary<int, int> a, Dictionary<int, int> b)
    {
        foreach (var item in a)
        {
            if (b.TryGetValue(item.Key, out var value) && value == item.Value)
                continue;
            return false;
        }

        return true;
    }
}