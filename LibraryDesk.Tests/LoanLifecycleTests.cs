using LibraryDesk.Core.Domain;

namespace LibraryDesk.Tests;

public class LoanLifecycleTests
{
    private static readonly DateTimeOffset Now =
        new(2026, 3, 10, 12, 0, 0, TimeSpan.Zero);

    private static Loan IssuedLoan()
    {
        var loan = new Loan(1, 10, Now);
        loan.AddItem(new LoanItem("ISBN-001", 7, 100m));
        loan.Issue(Now);
        return loan;
    }

    [Fact]
    public void Issue_NoItems_ThrowsInvalidOperation()
    {
        var loan = new Loan(1, 10, Now);

        Assert.Throws<InvalidOperationException>(() => loan.Issue(Now));
    }

    [Fact]
    public void MarkOverdue_ExactlyAtDueDate_ThrowsInvalidOperation()
    {
        Loan loan = IssuedLoan();

        Assert.Throws<InvalidOperationException>(
            () => loan.MarkOverdue(Now.AddDays(7)));
    }

    [Fact]
    public void MarkOverdue_AfterDueDate_SetsOverdue()
    {
        Loan loan = IssuedLoan();

        loan.MarkOverdue(Now.AddDays(7).AddSeconds(1));

        Assert.Equal(LoanStatus.Overdue, loan.Status);
    }

    [Fact]
    public void Return_FromOverdue_SetsReturned()
    {
        Loan loan = IssuedLoan();
        loan.MarkOverdue(Now.AddDays(8));

        loan.Return();

        Assert.Equal(LoanStatus.Returned, loan.Status);
    }

    [Fact]
    public void Cancel_AfterIssue_ThrowsInvalidOperation()
    {
        Loan loan = IssuedLoan();

        Assert.Throws<InvalidOperationException>(() => loan.Cancel());
    }
}
