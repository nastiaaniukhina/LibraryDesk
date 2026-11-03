using LibraryDesk.Core.Lending;
using Xunit;

namespace LibraryDesk.Tests;

public class LoanTotalCalculatorTests
{
    [Fact]
    public void Calculate_RegularReader_Returns1292()
    {
        LoanTotalRequest request = new(
            LoanId: 1,
            Reader: new ReaderInfo("Іваненко", "i@ex.com", true),
            Lines: new List<LoanLine>
            {
                new() { Isbn = "ISBN-001", Quantity = 12, UnitPrice = 100m },
                new() { Isbn = "ISBN-002", Quantity = 1, UnitPrice = 250m },
            },
            CreatedAt: new DateOnly(2026, 3, 10),
            CouponCode: null,
            Status: LoanFormStatus.Reserved,
            DeliveryPrice: 60m);

        LoanTotalResult result = LoanTotalCalculator.Calculate(request);

        Assert.Equal(1292.70m, result.Total);
        Assert.Equal(LoanFormStatus.Issued, result.Status);
    }

    [Fact]
    public void Calculate_SmallLoan_AddsDelivery()
    {
        LoanTotalRequest request = new(
            LoanId: 2,
            Reader: new ReaderInfo("Петренко", "p@ex.com", false),
            Lines: new List<LoanLine>
            {
                new() { Isbn = "ISBN-001", Quantity = 2, UnitPrice = 200m },
            },
            CreatedAt: new DateOnly(2026, 3, 10),
            CouponCode: "SALE10",
            Status: LoanFormStatus.Issued,
            DeliveryPrice: 60m);

        LoanTotalResult result = LoanTotalCalculator.Calculate(request);

        Assert.Equal(420m, result.Total);
        Assert.Equal(LoanFormStatus.Returned, result.Status);
    }

    [Fact]
    public void Calculate_CancelledLoan_ReturnsZero()
    {
        LoanTotalRequest request = new(
            LoanId: 3,
            Reader: new ReaderInfo("Коваль", "k@ex.com", false),
            Lines: new List<LoanLine>
            {
                new() { Isbn = "ISBN-001", Quantity = 2, UnitPrice = 200m },
            },
            CreatedAt: new DateOnly(2026, 3, 10),
            CouponCode: null,
            Status: LoanFormStatus.Cancelled,
            DeliveryPrice: 60m);

        LoanTotalResult result = LoanTotalCalculator.Calculate(request);

        Assert.Equal(0m, result.Total);
        Assert.Equal(LoanFormStatus.Cancelled, result.Status);
    }
}
