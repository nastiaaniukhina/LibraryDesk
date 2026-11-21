using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Pricing;

namespace LibraryDesk.Tests;

public class TieredPenaltyPolicyTests
{
    private readonly TieredPenaltyPolicy _policy = new();

    [Theory]
    [InlineData(1, false, 1.0)]
    [InlineData(7, false, 7.0)]
    [InlineData(8, false, 9.0)]
    [InlineData(10, false, 13.0)]
    [InlineData(1, true, 0.5)]
    [InlineData(7, true, 3.5)]
    [InlineData(8, true, 4.5)]
    [InlineData(10, true, 6.5)]
    public void PenaltyOf_ReturnsExpectedAmount(
        int overdueDays,
        bool isStudent,
        decimal expected)
    {
        LoanItem item = new("ISBN", 10, 2m);

        decimal result = _policy.PenaltyOf(item, overdueDays, isStudent);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PenaltyOf_ZeroDays_ReturnsZero(bool isStudent)
    {
        LoanItem item = new("ISBN", 10, 2m);

        decimal result = _policy.PenaltyOf(item, 0, isStudent);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void PenaltyOf_DoesNotExceedItemAmount()
    {
        LoanItem item = new("ISBN", 2, 1m);

        decimal result = _policy.PenaltyOf(item, 30, false);

        Assert.Equal(item.Amount, result);
    }

    [Fact]
    public void PenaltyOf_NegativeDays_ThrowsArgumentOutOfRangeException()
    {
        LoanItem item = new("ISBN", 10, 2m);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _policy.PenaltyOf(item, -1, false));
    }

    [Fact]
    public void PenaltyOf_NullItem_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _policy.PenaltyOf(null!, 1, false));
    }
}
