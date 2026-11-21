using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Reports;

namespace LibraryDesk.Core.Services;

/// <summary>Виконує пошук видач і побудову звітів.</summary>
public sealed class LoanQueryService
{
    private readonly ILoanRepository _repository;
    private readonly IPricingCatalog _pricing;
    private readonly TimeProvider _time;

    /// <summary>Створює сервіс запитів.</summary>
    public LoanQueryService(ILoanRepository repository, TimeProvider time, IPricingCatalog pricing)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _time = time ?? throw new ArgumentNullException(nameof(time));
        _pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
    }

    /// <summary>Шукає видачі читача.</summary>
    public IReadOnlyList<Loan> FindByReader(int readerId) =>
        _repository.GetAll().Where(loan => loan.ReaderId == readerId).ToList();

    /// <summary>Шукає видачі за ISBN.</summary>
    public IReadOnlyList<Loan> FindByIsbn(string isbn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(isbn);
        return _repository.GetAll()
            .Where(loan => loan.Items.Any(item => string.Equals(item.Isbn, isbn, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    /// <summary>Формує звіт боржників на дату.</summary>
    public IReadOnlyList<DebtorLine> OverdueReport(DateOnly asOf) =>
        OverdueReportBuilder.Build(_repository.GetAll(), asOf, _pricing);

    /// <summary>Формує звіт на поточну дату UTC.</summary>
    public IReadOnlyList<DebtorLine> OverdueReport() =>
        OverdueReport(DateOnly.FromDateTime(_time.GetUtcNow().UtcDateTime));
}