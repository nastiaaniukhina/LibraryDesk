using LibraryDesk.Core.Lending;
using Xunit;

namespace LibraryDesk.Tests;

public class LoanRulesTests
{
    [Fact]
    public void NextStatusOf_ReservedWithPositiveTotal_ReturnsIssued()
    {
        LoanFormStatus next = LoanTotalCalculator.NextStatusOf(
            LoanFormStatus.Reserved, 100m);

        Assert.Equal(LoanFormStatus.Issued, next);
    }

    [Fact]
    public void NextStatusOf_Issued_ReturnsReturned()
    {
        LoanFormStatus next = LoanTotalCalculator.NextStatusOf(
            LoanFormStatus.Issued, 100m);

        Assert.Equal(LoanFormStatus.Returned, next);
    }

    [Fact]
    public void VolumeDiscountOf_Above5000_ReturnsTenPercent()
    {
        decimal discount = LoanTotalCalculator.VolumeDiscountOf(6000m);

        Assert.Equal(600m, discount);
    }
}
