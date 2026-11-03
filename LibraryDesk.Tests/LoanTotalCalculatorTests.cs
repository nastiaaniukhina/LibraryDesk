using LibraryDesk.Core.Lending;
using Xunit;

namespace LibraryDesk.Tests;

public class LoanTotalCalculatorTests
{
    [Fact]
    public void Process_RegularReader_Returns1292()
    {
        List<LoanLine> lines = new()
        {
            new LoanLine { Isbn = "ISBN-001", Quantity = 12, UnitPrice = 100m },
            new LoanLine { Isbn = "ISBN-002", Quantity = 1, UnitPrice = 250m },
        };

        decimal total = LoanTotalCalculator.Process(
            1, "Іваненко", "i@ex.com", true, lines,
            new DateOnly(2026, 3, 10), null, LoanFormStatus.Reserved,
            false, 60m, out LoanFormStatus next);

        Assert.Equal(1292.70m, total);
        Assert.Equal(LoanFormStatus.Issued, next);
    }

    [Fact]
    public void Process_SmallLoan_AddsDelivery()
    {
        List<LoanLine> lines = new()
        {
            new LoanLine { Isbn = "ISBN-001", Quantity = 2, UnitPrice = 200m },
        };

        decimal total = LoanTotalCalculator.Process(
            2, "Петренко", "p@ex.com", false, lines,
            new DateOnly(2026, 3, 10), "SALE10", LoanFormStatus.Issued,
            false, 60m, out LoanFormStatus next);

        Assert.Equal(420m, total);
        Assert.Equal(LoanFormStatus.Returned, next);
    }
}
