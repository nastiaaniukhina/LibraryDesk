
using LibraryDesk.Core.Domain;
using Xunit;

namespace LibraryDesk.Tests;

public class ArchivedLoanTests
{
    [Fact]
    public void Constructor_WithNullLoan_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new ArchivedLoan(null!, DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Constructor_WithReservedLoan_ThrowsArgumentException()
    {
        var loan = new Loan(1, 1, DateTimeOffset.UtcNow);

        Assert.Throws<ArgumentException>(
            () => new ArchivedLoan(loan, DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Constructor_WithIssuedLoan_ThrowsArgumentException()
    {
        var loan = new Loan(1, 1, DateTimeOffset.UtcNow);
        loan.AddItem(new LoanItem("ISBN1", 7, 10m));
        loan.Issue(DateTimeOffset.UtcNow);

        Assert.Throws<ArgumentException>(
            () => new ArchivedLoan(loan, DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Constructor_WithReturnedLoan_CopiesLoanData()
    {
        var createdAt = DateTimeOffset.UtcNow;
        var archivedAt = createdAt.AddDays(10);
        var loan = new Loan(1, 2, createdAt);
        var item = new LoanItem("ISBN1", 7, 10m);

        loan.AddItem(item);
        loan.Issue(createdAt);
        loan.Return();

        var archived = new ArchivedLoan(loan, archivedAt);

        Assert.Equal(archivedAt, archived.ArchivedAt);
        Assert.Equal(1, archived.Id);
        Assert.Equal(LoanStatus.Returned, archived.Status);
        Assert.Single(archived.Items);
        Assert.Same(item, archived.Items[0]);
    }

    [Fact]
    public void Constructor_WithCancelledLoan_CreatesArchive()
    {
        var loan = new Loan(2, 3, DateTimeOffset.UtcNow);
        loan.Cancel();

        var archived = new ArchivedLoan(loan, DateTimeOffset.UtcNow);

        Assert.Equal(2, archived.Id);
        Assert.Equal(LoanStatus.Cancelled, archived.Status);
        Assert.Empty(archived.Items);
    }
}
