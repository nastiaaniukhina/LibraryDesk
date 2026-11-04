using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Notifications;
using LibraryDesk.Core.Pricing;
using LibraryDesk.Core.Services;
using LibraryDesk.Core.Storage;
using Xunit;

namespace LibraryDesk.Tests;

public class DomainInvariantTests
{
    [Fact]
    public void LoanItem_WithZeroDays_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new LoanItem("ISBN-001", 0, 1m));
    }

    [Fact]
    public void Loan_AfterIssue_AddItem_ThrowsInvalidOperation()
    {
        Loan loan = NewLoan(1);
        loan.AddItem(new LoanItem("ISBN-001", 7, 1m));
        loan.Issue(DateTimeOffset.UtcNow);

        Assert.Throws<InvalidOperationException>(
            () => loan.AddItem(new LoanItem("ISBN-002", 1, 1m)));
    }

    [Fact]
    public void TotalOf_WithTenPercentDiscount_Returns180()
    {
        LoanService service = new(
            new InMemoryLoanRepository(),
            new DiscountFeePolicy(0.10m),
            new TextWriterNotifier(new StringWriter()));
        Loan loan = NewLoan(2);
        loan.AddItem(new LoanItem("ISBN-001", 2, 100m));

        Assert.Equal(180m, service.TotalOf(loan));
    }

    private static Loan NewLoan(int id) => new(id, 10, DateTimeOffset.UtcNow);
}
