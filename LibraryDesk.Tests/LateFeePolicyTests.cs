using LibraryDesk.Core.Pricing;

namespace LibraryDesk.Tests;

public class LateFeePolicyTests
{
    [Theory]
    [InlineData(0, false, 0)]
    [InlineData(3, false, 0)]
    [InlineData(4, false, 2)]
    [InlineData(13, false, 20)]
    [InlineData(14, false, 33)]
    [InlineData(14, true, 16.5)]
    [InlineData(10, true, 7)]
    public void FeeFor_Boundaries_MatchesTable(
        int daysOverdue, bool isStudent, double expected)
    {
        var policy = new LateFeePolicy();

        decimal actual = LateFeePolicy.FeeFor(daysOverdue, isStudent);

        Assert.Equal((decimal)expected, actual);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void FeeFor_NegativeDays_ThrowsOutOfRange(int daysOverdue)
    {
        var policy = new LateFeePolicy();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => LateFeePolicy.FeeFor(daysOverdue, true));

        Assert.Equal("daysOverdue", ex.ParamName);
    }
        [Fact]
    public void FeeFor_LongOverdue_IsCappedAtBookPrice()
    {
        var policy = new LateFeePolicy();

        decimal actual = LateFeePolicy.FeeFor(60, false, 50m);

        Assert.Equal(50m, actual);
    }
}

