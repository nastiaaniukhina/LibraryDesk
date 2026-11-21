using System.Text.Json;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Storage;

/// <summary>JSON-сховище видач із явним збереженням змін.</summary>
public sealed class JsonLoanRepository : ILoanRepository
{
    private readonly string _path;
    private readonly List<Loan> _loans;

    /// <summary>Завантажує сховище з файлу або створює порожнє.</summary>
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
        if (_loans.Any(item => item.Id == loan.Id)) throw new InvalidOperationException("Видача з таким Id уже існує.");
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
        File.WriteAllText(_path, JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true }));
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