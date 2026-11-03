using LibraryDesk.Core.Lending;
using LibraryDesk.Core.Lending.Legacy;
using Xunit;

namespace LibraryDesk.Tests;

public class LegacyLoanProcessorTests
{
    [Fact]
    public void Process_RegularReader_Returns1292()
    {
        List<LoanLine> lines = new()
        {
            new LoanLine { Isbn = "ISBN-001", Quantity = 12, UnitPrice = 100m },
            new LoanLine { Isbn = "ISBN-002", Quantity = 1, UnitPrice = 250m },
        };

        decimal total = LegacyLoanProcessor.Process(
            1, "Іваненко", "i@ex.com", true, lines,
            new DateOnly(2026, 3, 10), string.Empty, "Reserved", false,
            60m, out string next);

        Assert.Equal(1292.70m, total);
        Assert.Equal("Issued", next);
    }

    [Fact]
    public void Process_SmallLoan_AddsDelivery()
    {
        List<LoanLine> lines = new()
        {
            new LoanLine { Isbn = "ISBN-001", Quantity = 2, UnitPrice = 200m },
        };

        decimal total = LegacyLoanProcessor.Process(
            2, "Петренко", "p@ex.com", false, lines,
            new DateOnly(2026, 3, 10), "SALE10", "Issued",
            false, 60m, out string next);

        Assert.Equal(420m, total);
        Assert.Equal("Returned", next);
    }

    [Fact]
    public void Process_NoLines_ReturnsMinusOne()
    {
        decimal total = LegacyLoanProcessor.Process(
            3, "Коваль", "k@ex.com", false,
            new List<LoanLine>(),
            new DateOnly(2026, 3, 10), string.Empty, "Reserved", false,
            60m, out string next);

        Assert.Equal(-1m, total);
    }
}