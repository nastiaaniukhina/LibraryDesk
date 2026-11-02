
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryDesk.Core.Lending;

/// <summary>
/// Видача книжок читачеві: заголовок документа та його рядки.
/// </summary>
public class BookLoan
{
    private readonly List<string[]> _lines = new List<string[]>();

    /// <summary>Створює нову видачу у початковому стані.</summary>
    /// <param name="id">Номер видачі.</param>
    /// <param name="readerName">Прізвище читача.</param>
    public BookLoan(string id, string readerName)
    {
        Id = id;
        ReaderName = readerName;
        CreatedAt = DateTime.Now;
    }

    /// <summary>Номер видачі.</summary>
    public string Id { get; private set; }

    /// <summary>Прізвище читача.</summary>
    public string ReaderName { get; private set; }

    /// <summary>Поточний стан видачі.</summary>
    public int Status { get; private set; }

    /// <summary>Дата й час створення видачі.</summary>
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
        string[] line = new string[3];
        line[0] = isbn;
        line[1] = quantity.ToString();
        line[2] = unitPrice.ToString();
        _lines.Add(line);
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
            int quantity = int.Parse(_lines[i][1]);
            decimal unitPrice = decimal.Parse(_lines[i][2]);
            total = total + (quantity * unitPrice);
            lineCount = lineCount + 1;
        }

        // Знижки не поєднуються: діє лише перша з підходящих
        if (isRegularReader == true && total > 1000)
        {
            total = total * 0.9m;
        }
        else if (total > 5000)
        {
            total = total * 0.85m;
        }

        if (lineCount > 10)
        {
            total = total - 100;
        }

        if (total < 0)
        {
            total = 0;
        }

        // ПДВ нараховується на суму вже після всіх знижок
        total = total + total * 0.2m;
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
        if (Status == 0 && newStatus == 1)
        {
            Status = 1;
            return true;
        }

        if (Status == 1 && newStatus == 2)
        {
            Status = 2;
            return true;
        }

        if (Status == 0 && newStatus == 3)
        {
            Status = 3;
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
            && ReaderName.Length > 2
            && _lines.Count > 0
            && _lines.Count < 100
            && Status >= 0
            && Status <= 3)
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
            report = report + "Книжка: " + _lines[i][0]
                + "; кількість: " + _lines[i][1]
                + "; ціна: " + _lines[i][2]
                + "; сума: " + (int.Parse(_lines[i][1]) * decimal.Parse(_lines[i][2])) + "\n";
        }

        report = report + "Разом: " + CalculateTotal(false) + "\n";
        return report;
    }
}
