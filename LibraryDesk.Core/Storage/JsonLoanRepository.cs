using System.Text.Json;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Storage;

/// <summary>JSON-СЃС…РѕРІРёС‰Рµ РІРёРґР°С‡ С–Р· СЏРІРЅРёРј Р·Р±РµСЂРµР¶РµРЅРЅСЏРј Р·РјС–РЅ.</summary>
public sealed class JsonLoanRepository : ILoanRepository
{
    private readonly string _path;
    private readonly List<Loan> _loans;
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    /// <summary>Р—Р°РІР°РЅС‚Р°Р¶СѓС” СЃС…РѕРІРёС‰Рµ Р· С„Р°Р№Р»Сѓ Р°Р±Рѕ СЃС‚РІРѕСЂСЋС” РїРѕСЂРѕР¶РЅС”.</summary>
    public JsonLoanRepository(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _path = path;
        if (!File.Exists(path))
        {
            _loans = new List<Loan>();
            return;
        }

        List<LoanRecord> records = JsonSerializer.Deserialize<List<LoanRecord>>(File.ReadAllText(path)) ?? new();
        _loans = records.Select(ToDomain).ToList();
    }

    /// <inheritdoc/>
    public void Add(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        if (_loans.Any(item => item.Id == loan.Id)) throw new InvalidOperationException("Р’РёРґР°С‡Р° Р· С‚Р°РєРёРј Id СѓР¶Рµ С–СЃРЅСѓС”.");
        _loans.Add(loan);
    }

    /// <inheritdoc/>
    public Loan? GetById(int id) => _loans.FirstOrDefault(item => item.Id == id);

    /// <inheritdoc/>
    public IReadOnlyList<Loan> GetAll() => _loans.ToList();

    /// <inheritdoc/>
    public void Save()
    {
        string? directory = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        List<LoanRecord> records = _loans.Select(FromDomain).ToList();
        File.WriteAllText(_path, JsonSerializer.Serialize(records, SerializerOptions));
    }

    private static LoanRecord FromDomain(Loan loan) => new(
        loan.Id, loan.ReaderId, loan.CreatedAt, loan.ReaderKind, loan.Status, loan.DueAt, loan.ReturnedAt,
        loan.Items.Select(item => new ItemRecord(item.Isbn, item.Days, item.DailyRate)).ToList());

    private static Loan ToDomain(LoanRecord record) => Loan.Restore(
        record.Id, record.ReaderId, record.CreatedAt, record.ReaderKind, record.Status, record.DueAt,
        record.ReturnedAt, record.Items.Select(item => new LoanItem(item.Isbn, item.Days, item.DailyRate)));

    private sealed record LoanRecord(
        int Id, int ReaderId, DateTimeOffset CreatedAt, ReaderKind ReaderKind, LoanStatus Status,
        DateTimeOffset? DueAt, DateTimeOffset? ReturnedAt, List<ItemRecord> Items);

    private sealed record ItemRecord(string Isbn, int Days, decimal DailyRate);
}
