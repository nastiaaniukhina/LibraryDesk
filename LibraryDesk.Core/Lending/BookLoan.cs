
namespace LibraryDesk.Core.Lending;

/// <summary>
/// Видача книжок читачеві: заголовок документа та його рядки.
/// </summary>
public class BookLoan
{
    /// <summary>Стан «заброньовано» (початковий).</summary>
    public const int StatusReserved = 0;

    /// <summary>Стан «видано».</summary>
    public const int StatusIssued = 1;

    /// <summary>Стан «повернено».</summary>
    public const int StatusReturned = 2;

    /// <summary>Стан «скасовано».</summary>
    public const int StatusCancelled = 3;

    private const decimal VatRate = 0.2m;
    private const decimal RegularDiscountThreshold = 1000m;
    private const decimal RegularDiscountRate = 0.10m;
    private const decimal LargeOrderThreshold = 5000m;
    private const decimal LargeOrderDiscountRate = 0.15m;
    private const int BulkLineCount = 10;
    private const decimal BulkDiscountAmount = 100m;
    private const int MaxLineCount = 100;
    private const int MinReaderNameLength = 3;

    private readonly List<(string Isbn, int Quantity, decimal UnitPrice)> _lines = new();

    /// <summary>Initializes a new instance of the <see cref="BookLoan"/> class.</summary>
    /// <param name="id">Номер видачі.</param>
    /// <param name="readerName">Прізвище читача.</param>
    public BookLoan(string id, string readerName)
    {
        Id = id;
        ReaderName = readerName;
        CreatedAt = DateTime.Now;
    }

    /// <summary>Gets the loan number.</summary>
    public string Id { get; private set; }

    /// <summary>Gets the reader's surname.</summary>
    public string ReaderName { get; private set; }

    /// <summary>Gets the current loan status.</summary>
    public int Status { get; private set; }

    /// <summary>Gets the loan creation date and time.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Шукає видачу за номером.</summary>
    /// <param name="loans">Список видач, у якому виконується пошук.</param>
    /// <param name="loanId">Номер видачі, яку шукають.</param>
    /// <returns>Знайдена видача або null, якщо такої немає.</returns>
    public static BookLoan? FindById(List<BookLoan> loans, string loanId)
    {
        for (int i = 0; i < loans.Count; i++)
        {
            if (loans[i].Id == loanId)
            {
                return loans[i];
            }
        }

        return null;
    }

    /// <summary>Додає до видачі рядок із примірником книжки.</summary>
    /// <param name="isbn">Код книжки (ISBN).</param>
    /// <param name="quantity">Кількість одиниць.</param>
    /// <param name="unitPrice">Ціна за одиницю, грн.</param>
    public void AddLine(string isbn, int quantity, decimal unitPrice)
    {
        _lines.Add((isbn, quantity, unitPrice));
    }

    /// <summary>
    /// Обчислює підсумкову суму видачі з урахуванням знижок і ПДВ.
    /// </summary>
    /// <param name="isRegularReader">Ознака постійного читача.</param>
    /// <returns>Сума до сплати, заокруглена до двох знаків.</returns>
    public decimal CalculateTotal(bool isRegularReader)
    {
        decimal total = 0;
        int lineCount = 0;

        for (int i = 0; i < _lines.Count; i++)
        {
            total += _lines[i].Quantity * _lines[i].UnitPrice;
            lineCount++;
        }

        // Знижки не поєднуються: діє лише перша з підходящих
        if (isRegularReader && total > RegularDiscountThreshold)
        {
            total *= 1m - RegularDiscountRate;
        }
        else if (total > LargeOrderThreshold)
        {
            total *= 1m - LargeOrderDiscountRate;
        }

        if (lineCount > BulkLineCount)
        {
            total -= BulkDiscountAmount;
        }

        if (total < 0)
        {
            total = 0;
        }

        // ПДВ нараховується на суму вже після всіх знижок
        total += total * VatRate;
        return Math.Round(total, 2);
    }

    /// <summary>
    /// Змінює стан видачі, якщо перехід дозволений правилами.
    /// </summary>
    /// <param name="newStatus">Цільовий стан видачі.</param>
    /// <returns>
    /// true, якщо перехід виконано; false, якщо перехід заборонений правилами.
    /// </returns>
    public bool TryChangeStatus(int newStatus)
    {
        if (Status == StatusReserved && newStatus == StatusIssued)
        {
            Status = StatusIssued;
            return true;
        }

        if (Status == StatusIssued && newStatus == StatusReturned)
        {
            Status = StatusReturned;
            return true;
        }

        if (Status == StatusReserved && newStatus == StatusCancelled)
        {
            Status = StatusCancelled;
            return true;
        }

        return false;
    }

    /// <summary>Перевіряє, чи видача заповнена коректно.</summary>
    /// <returns>true, якщо всі обов'язкові поля заповнені правильно.</returns>
    public bool IsValid()
    {
        if (Id is not null && Id.Length > 0
            && ReaderName is not null
            && ReaderName.Length >= MinReaderNameLength
            && _lines.Count > 0
            && _lines.Count < MaxLineCount
            && Status >= StatusReserved
            && Status <= StatusCancelled)
        {
            return true;
        }

        return false;
    }

    /// <summary>Формує текстовий звіт про видачу.</summary>
    /// <returns>Звіт: по рядку на кожен примірник і підсумок.</returns>
    public string BuildReport()
    {
        string report = string.Empty;

        for (int i = 0; i < _lines.Count; i++)
        {
            report = report + "Книжка: " + _lines[i].Isbn
                + "; кількість: " + _lines[i].Quantity
                + "; ціна: " + _lines[i].UnitPrice
                + "; сума: " + (_lines[i].Quantity * _lines[i].UnitPrice) + "\n";
        }

        report = report + "Разом: " + CalculateTotal(false) + "\n";
        return report;
    }
}
