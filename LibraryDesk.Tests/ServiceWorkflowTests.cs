using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Notifications;
using LibraryDesk.Core.Pricing;
using LibraryDesk.Core.Services;
using LibraryDesk.Core.Storage;

namespace LibraryDesk.Tests;

public sealed class ServiceWorkflowTests
{
    [Fact]
    public void Issue_ReservedLoan_ChangesStatus()
    {
        InMemoryLoanRepository repository = new();
        Loan loan = new(1, 7, DateTimeOffset.Parse("2026-01-01T00:00:00Z"));
        loan.AddItem(new LoanItem("ISBN-1", 7, 1m));
        repository.Add(loan);
        LoanLifecycleService service = new(repository, PricingCatalog.Default(),
            new TextWriterNotifier(TextWriter.Null), TimeProvider.System);

        service.Issue(1);

        Assert.Equal(LoanStatus.Issued, loan.Status);
        Assert.NotNull(loan.DueAt);
    }

    [Fact]
    public void Cancel_ReservedLoan_ChangesStatus()
    {
        InMemoryLoanRepository repository = new();
        Loan loan = new(2, 7, DateTimeOffset.Parse("2026-01-01T00:00:00Z"));
        loan.AddItem(new LoanItem("ISBN-2", 2, 1m));
        repository.Add(loan);
        LoanLifecycleService service = new(repository, PricingCatalog.Default(),
            new TextWriterNotifier(TextWriter.Null), TimeProvider.System);

        service.Cancel(2);

        Assert.Equal(LoanStatus.Cancelled, loan.Status);
    }

    [Fact]
    public void FindByIsbn_ReturnsMatchingLoan()
    {
        InMemoryLoanRepository repository = new();
        Loan loan = new(3, 7, DateTimeOffset.Parse("2026-01-01T00:00:00Z"));
        loan.AddItem(new LoanItem("ISBN-3", 2, 1m));
        repository.Add(loan);
        LoanQueryService service = new(repository, TimeProvider.System, PricingCatalog.Default());

        Assert.Single(service.FindByIsbn("ISBN-3"));
    }
}
