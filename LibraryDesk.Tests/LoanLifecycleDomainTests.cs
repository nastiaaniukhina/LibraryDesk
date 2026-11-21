using LibraryDesk.Core.Domain;

namespace LibraryDesk.Tests;

public class LoanLifecycleDomainTests
{
    [Fact]
    public void OverdueDaysAt_NoDueDate_ReturnsZero()
    {
        Loan loan = NewLoan();

        Assert.Equal(0, loan.OverdueDaysAt(new DateOnly(2026, 11, 5)));
    }

    [Fact]
    public void OverdueDaysAt_OnDueDate_ReturnsZero()
    {
        Loan loan = IssuedLoan();

        Assert.Equal(0, loan.OverdueDaysAt(new DateOnly(2026, 11, 3)));
    }

    [Fact]
    public void OverdueDaysAt_AfterDueDate_ReturnsCalendarDayDifference()
    {
        Loan loan = IssuedLoan();

        Assert.Equal(2, loan.OverdueDaysAt(new DateOnly(2026, 11, 5)));
    }

    [Fact]
    public void OverdueDaysAt_BeforeDueDate_ReturnsZero()
    {
        Loan loan = IssuedLoan();

        Assert.Equal(0, loan.OverdueDaysAt(new DateOnly(2026, 11, 2)));
    }

    [Fact]
    public void Return_IssuedLoan_SetsReturnedAtAndStatus()
    {
        Loan loan = IssuedLoan();
        DateTimeOffset returnedAt = new(2026, 11, 5, 12, 30, 0, TimeSpan.Zero);

        loan.Return(returnedAt);

        Assert.Equal(returnedAt, loan.ReturnedAt);
        Assert.Equal(LoanStatus.Returned, loan.Status);
    }

    private static Loan NewLoan()
    {
        return new Loan(1, 10, new DateTimeOffset(2026, 11, 1, 10, 0, 0, TimeSpan.Zero));
    }

    private static Loan IssuedLoan()
    {
        Loan loan = NewLoan();
        loan.AddItem(new LoanItem("ISBN-001", 2, 100m));
        loan.Issue(new DateTimeOffset(2026, 11, 1, 10, 0, 0, TimeSpan.Zero));
        return loan;
    }
}
