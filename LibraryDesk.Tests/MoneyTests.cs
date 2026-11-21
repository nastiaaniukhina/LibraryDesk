using LibraryDesk.Core.Domain;

namespace LibraryDesk.Tests;

public class MoneyTests
{
    [Theory]
    [InlineData(1.005, 1.01)]
    [InlineData(1.004, 1.00)]
    [InlineData(1.999, 2.00)]
    [InlineData(-1.005, -1.01)]
    public void Round_UsesAwayFromZero(decimal amount, decimal expected)
    {
        Assert.Equal(expected, Money.Round(amount));
    }

    [Fact]
    public void Digits_ReturnsTwo()
    {
        Assert.Equal(2, Money.Digits);
    }
}
