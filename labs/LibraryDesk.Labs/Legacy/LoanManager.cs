#pragma warning disable
// ЛЕГАСІ-КОД ДЛЯ АУДИТУ SOLID (ЛР 4). Не використовується в застосунку.
namespace LibraryDesk.Legacy;

public class LoanManager
{
    private readonly FileLoanStore _store = new FileLoanStore();
    private readonly SmtpMailer _mailer = new SmtpMailer();

    public decimal Place(LegacyLoan loan, string readerType)
    {
        decimal total = 0m;
        foreach (var item in loan.Items)
            total += item.DailyRate * item.Days;

        if (readerType == "regular")
            total = total * 0.95m;
        else if (readerType == "student")
            total = total * 0.90m;
        else if (readerType == "staff")
            total = total * 0.70m;

        if (total > 1000m)
            total = total - 50m;

        loan.Total = total;
        loan.Status = 1;

        _store.SaveToFile(loan, "loans.json");
        _mailer.Send(loan.Email, "Loan " + loan.Id, "" + total);
        return total;
    }

    public string BuildCsvReport(List<LegacyLoan> loans)
    {
        var text = "id;total;status" + Environment.NewLine;
        foreach (var o in loans)
            text += o.Id + ";" + o.Total + ";" + o.Status;

        File.WriteAllText("report.csv", text);
        Console.WriteLine("Звіт збережено");
        return text;
    }

    public void Archive(LegacyLoan loan)
    {
        LegacyLoan archived = new LegacyArchivedLoan(loan.Id);
        foreach (var item in loan.Items)
            archived.AddItem(item);

        _store.SaveToFile(archived, "archive.json");
    }
}
