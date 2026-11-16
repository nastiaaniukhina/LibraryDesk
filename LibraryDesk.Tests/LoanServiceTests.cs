using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Services;

namespace LibraryDesk.Tests;

public class LoanServiceTests
{
    private static Loan NewLoan(int id, int readerId) =>
        new(id, readerId, new DateTimeOffset(2026, 3, 10, 12, 0, 0, TimeSpan.Zero));

    [Fact]
    public void TotalOf_TwoItems_SumsPricesFromPolicy()
    {
        var service = new LoanService(
            new FakeLoanRepository(), new StubFeePolicy(25m), new SpyNotifier());
        Loan loan = NewLoan(1, 10);
        loan.AddItem(new LoanItem("ISBN-001", 7, 100m));
        loan.AddItem(new LoanItem("ISBN-002", 3, 300m));

        decimal actual = service.TotalOf(loan);

        Assert.Equal(50m, actual);
    }

    [Fact]
    public void Place_ValidLoan_StoresInRepositoryOnce()
    {
        var repository = new FakeLoanRepository();
        var service = new LoanService(
            repository, new StubFeePolicy(1m), new SpyNotifier());
        Loan loan = NewLoan(1, 10);
        var reader = new Reader(10, "Іваненко", "i@ex.com");

        service.Place(loan, reader);

        Assert.NotNull(repository.GetById(1));
        Assert.Equal(1, repository.AddCalls);
    }

    [Fact]
    public void Place_ValidLoan_NotifiesReaderEmail()
    {
        var notifier = new SpyNotifier();
        var service = new LoanService(
            new FakeLoanRepository(), new StubFeePolicy(1m), notifier);
        var reader = new Reader(10, "Іваненко", "i@ex.com");

        service.Place(NewLoan(1, 10), reader);

        var sent = Assert.Single(notifier.Sent);
        Assert.Equal("i@ex.com", sent.To);
    }

    [Fact]
    public void Place_LoanOfAnotherReader_ThrowsArgument()
    {
        var service = new LoanService(
            new FakeLoanRepository(), new StubFeePolicy(1m), new SpyNotifier());
        var reader = new Reader(99, "Петренко", "p@ex.com");

        var ex = Assert.Throws<ArgumentException>(
            () => service.Place(NewLoan(1, 10), reader));

        Assert.Equal("reader", ex.ParamName);
    }
}
