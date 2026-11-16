using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Pricing;

namespace LibraryDesk.Tests;

public class StandardFeePolicyTests
{
    [Fact]
    public void PriceOf_ValidItem_ReturnsRoundedAmount()
    {
        var policy = new StandardFeePolicy();
        var item = new LoanItem("978-617-000-000-0", 3, 3.376m);

        decimal result = policy.PriceOf(item);

        Assert.Equal(10.13m, result);
    }

    [Fact]
    public void PriceOf_NullItem_ThrowsArgumentNullException()
    {
        var policy = new StandardFeePolicy();

        Assert.Throws<ArgumentNullException>(() => policy.PriceOf(null!));
    }
}
