using System.Text.Json;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Storage;

/// <summary>JSON-Р РЋР С“Р РЋРІР‚В¦Р В РЎвЂўР В Р вЂ Р В РЎвЂР РЋРІР‚В°Р В Р’Вµ Р В Р вЂ Р В РЎвЂР В РўвЂР В Р’В°Р РЋРІР‚РЋ Р РЋРІР‚вЂњР В Р’В· Р РЋР РЏР В Р вЂ Р В Р вЂ¦Р В РЎвЂР В РЎВ Р В Р’В·Р В Р’В±Р В Р’ВµР РЋР вЂљР В Р’ВµР В Р’В¶Р В Р’ВµР В Р вЂ¦Р В Р вЂ¦Р РЋР РЏР В РЎВ Р В Р’В·Р В РЎВР РЋРІР‚вЂњР В Р вЂ¦.</summary>
public sealed class JsonLoanRepository : ILoanRepository
{
    private readonly string _path;
    private readonly List<Loan> _loans;
    private static readonly JsonSerializerOptions _serializerOptions = new() { WriteIndented = true };

    /// <summary>Р В РІР‚вЂќР В Р’В°Р В Р вЂ Р В Р’В°Р В Р вЂ¦Р РЋРІР‚С™Р В Р’В°Р В Р’В¶Р РЋРЎвЂњР РЋРІР‚Сњ Р РЋР С“Р РЋРІР‚В¦Р В РЎвЂўР В Р вЂ Р В РЎвЂР РЋРІР‚В°Р В Р’Вµ Р В Р’В· Р РЋРІР‚С›Р В Р’В°Р В РІвЂћвЂ“Р В Р’В»Р РЋРЎвЂњ Р В Р’В°Р В Р’В±Р В РЎвЂў Р РЋР С“Р РЋРІР‚С™Р В Р вЂ Р В РЎвЂўР РЋР вЂљР РЋР вЂ№Р РЋРІР‚Сњ Р В РЎвЂ”Р В РЎвЂўР РЋР вЂљР В РЎвЂўР В Р’В¶Р В Р вЂ¦Р РЋРІР‚Сњ.</summary>
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
        if (_loans.Any(item => item.Id == loan.Id))
        {
            throw new InvalidOperationException("Р В РІР‚в„ўР В РЎвЂР В РўвЂР В Р’В°Р РЋРІР‚РЋР В Р’В° Р В Р’В· Р РЋРІР‚С™Р В Р’В°Р В РЎвЂќР В РЎвЂР В РЎВ Id Р РЋРЎвЂњР В Р’В¶Р В Р’Вµ Р РЋРІР‚вЂњР РЋР С“Р В Р вЂ¦Р РЋРЎвЂњР РЋРІР‚Сњ.");
        }

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
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        List<LoanRecord> records = _loans.Select(FromDomain).ToList();
        File.WriteAllText(_path, JsonSerializer.Serialize(records, _serializerOptions));
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

