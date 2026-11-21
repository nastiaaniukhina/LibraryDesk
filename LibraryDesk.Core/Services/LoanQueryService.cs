using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Reports;

namespace LibraryDesk.Core.Services;

/// <summary>Р’РёРєРѕРЅСѓС” РїРѕС€СѓРє РІРёРґР°С‡ С– РїРѕР±СѓРґРѕРІСѓ Р·РІС–С‚С–РІ.</summary>
public sealed class LoanQueryService
{
    private readonly ILoanRepository _repository;
    private readonly IPricingCatalog _pricing;
    private readonly TimeProvider _time;

    /// <summary>РЎС‚РІРѕСЂСЋС” СЃРµСЂРІС–СЃ Р·Р°РїРёС‚С–РІ.</summary>
    public LoanQueryService(ILoanRepository repository, TimeProvider time, IPricingCatalog pricing)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _time = time ?? throw new ArgumentNullException(nameof(time));
        _pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
    }

    /// <summary>РЁСѓРєР°С” РІРёРґР°С‡С– С‡РёС‚Р°С‡Р°.</summary>
    public IReadOnlyList<Loan> FindByReader(int readerId) =>
        _repository.GetAll().Where(loan => loan.ReaderId == readerId).ToList();

    /// <summary>РЁСѓРєР°С” РІРёРґР°С‡С– Р·Р° ISBN.</summary>
    public IReadOnlyList<Loan> FindByIsbn(string isbn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(isbn);
        return _repository.GetAll()
            .Where(loan => loan.Items.Any(item => string.Equals(item.Isbn, isbn, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    /// <summary>Р¤РѕСЂРјСѓС” Р·РІС–С‚ Р±РѕСЂР¶РЅРёРєС–РІ РЅР° РґР°С‚Сѓ.</summary>
    public IReadOnlyList<DebtorLine> OverdueReport(DateOnly asOf) =>
        OverdueReportBuilder.Build(_repository.GetAll(), asOf, _pricing);

    /// <summary>Р¤РѕСЂРјСѓС” Р·РІС–С‚ РЅР° РїРѕС‚РѕС‡РЅСѓ РґР°С‚Сѓ UTC.</summary>
    public IReadOnlyList<DebtorLine> OverdueReport() =>
        OverdueReport(DateOnly.FromDateTime(_time.GetUtcNow().UtcDateTime));
}
