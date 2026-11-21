using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Pricing;
using LibraryDesk.Core.Reports;
using LibraryDesk.Core.Services;

namespace LibraryDesk.Tests;

public class CoverageTests
{
    private static readonly DateTimeOffset Start =
        new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Book_TakeCopyAndReturnCopy_ChangesAvailableCopies()
    {
        Book book = new("ISBN-1", "Title", "Author", 5m, 2);

        book.TakeCopy();
        Assert.Equal(1, book.CopiesAvailable);

        book.ReturnCopy();
        Assert.Equal(2, book.CopiesAvailable);
    }

    [Fact]
    public void Book_TakeCopyWithoutCopies_ThrowsInvalidOperation()
    {
        Book book = new("ISBN-1", "Title", "Author", 5m, 0);

        Assert.Throws<InvalidOperationException>(() => book.TakeCopy());
    }

    [Fact]
    public void Book_InvalidArguments_ThrowExceptions()
    {
        Assert.Throws<ArgumentException>(() => new Book(string.Empty, "Title", "Author", 5m, 1));
        Assert.Throws<ArgumentException>(() => new Book("ISBN", string.Empty, "Author", 5m, 1));
        Assert.Throws<ArgumentException>(() => new Book("ISBN", "Title", string.Empty, 5m, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Book("ISBN", "Title", "Author", -1m, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Book("ISBN", "Title", "Author", 5m, -1));
    }

    [Fact]
    public void Reader_ValidData_CreatesReader()
    {
        Reader reader = new(1, "Test Reader", "test@example.com")
        {
            IsStudent = true,
        };

        Assert.Equal(1, reader.Id);
        Assert.Equal("Test Reader", reader.Name);
        Assert.Equal("test@example.com", reader.Email);
        Assert.True(reader.IsStudent);
    }

    [Fact]
    public void Reader_InvalidData_ThrowsExceptions()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Reader(0, "Name", "a@b.com"));
        Assert.Throws<ArgumentException>(() => new Reader(1, string.Empty, "a@b.com"));
        Assert.Throws<ArgumentException>(() => new Reader(1, "Name", string.Empty));
        Assert.Throws<ArgumentException>(() => new Reader(1, "Name", "invalid-email"));
    }

    [Fact]
    public void ArchivedLoan_CancelledLoan_StoresLoanData()
    {
        Loan loan = new(1, 10, Start);
        loan.Cancel();

        DateTimeOffset archivedAt = Start.AddDays(1);
        ArchivedLoan archived = new(loan, archivedAt);

        Assert.Equal(1, archived.Id);
        Assert.Equal(LoanStatus.Cancelled, archived.Status);
        Assert.Equal(archivedAt, archived.ArchivedAt);
        Assert.Empty(archived.Items);
    }

    [Fact]
    public void ArchivedLoan_ReturnedLoan_CanBeArchived()
    {
        Loan loan = new(2, 10, Start);
        loan.AddItem(new LoanItem("ISBN-2", 2, 5m));
        loan.Issue(Start);
        loan.Return(Start.AddDays(1));

        ArchivedLoan archived = new(loan, Start.AddDays(2));

        Assert.Equal(LoanStatus.Returned, archived.Status);
        Assert.Single(archived.Items);
    }

    [Fact]
    public void ArchivedLoan_InvalidLoan_ThrowsExceptions()
    {
        Assert.Throws<ArgumentNullException>(
            () => new ArchivedLoan(null!, Start));

        Loan loan = new(3, 10, Start);

        Assert.Throws<ArgumentException>(
            () => new ArchivedLoan(loan, Start));
    }

    [Fact]
    public void ReturnReceipt_StoresValues()
    {
        ReturnReceipt receipt = new(100m, 15m, 3);

        Assert.Equal(100m, receipt.Total);
        Assert.Equal(15m, receipt.Penalty);
        Assert.Equal(3, receipt.OverdueDays);
    }

    [Fact]
    public void LoanNotFoundException_StoresMessage()
    {
        LoanNotFoundException exception = new("Loan not found");

        Assert.Equal("Loan not found", exception.Message);
    }

    [Fact]
    public void StandardFeePolicy_ReturnsItemAmount()
    {
        LoanItem item = new("ISBN-4", 5, 10m);
        StandardFeePolicy policy = new();

        Assert.Equal(item.Amount, policy.PriceOf(item));
    }

    [Fact]
    public void StandardFeePolicy_NullItem_ThrowsArgumentNull()
    {
        StandardFeePolicy policy = new();

        Assert.Throws<ArgumentNullException>(() => policy.PriceOf(null!));
    }

    [Fact]
    public void LoanCsvReport_BuildCsv_ContainsHeaderAndLoanData()
    {
        Loan loan = new(4, 10, Start);
        loan.AddItem(new LoanItem("ISBN-4", 2, 5m));

        string csv = LoanCsvReport.BuildCsv(new[] { loan }, _ => 12.5m);

        Assert.Contains("id;total;status", csv);
        Assert.Contains("4;12.50;Reserved", csv);
    }

    [Fact]
    public void LoanCsvReport_NullArguments_ThrowArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(
            () => LoanCsvReport.BuildCsv(null!, _ => 0m));

        Assert.Throws<ArgumentNullException>(
            () => LoanCsvReport.BuildCsv(Array.Empty<Loan>(), null!));
    }

    [Fact]
    public void OverdueReportFormatter_FormatsDebtors()
    {
        DebtorLine[] lines =
        {
            new(1, 10, 5, 12.5m),
            new(2, 20, 3, 7m),
        };

        string result = OverdueReportFormatter.Format(lines);

        Assert.Contains("#1", result);
        Assert.Contains("#10", result);
        Assert.Contains("12.50", result);
        Assert.Contains("#2", result);
        Assert.Contains("7.00", result);
    }

    [Fact]
    public void OverdueReportFormatter_EmptyList_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, OverdueReportFormatter.Format(Array.Empty<DebtorLine>()));
    }

    [Fact]
    public void OverdueReportFormatter_NullList_ThrowsArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(
            () => OverdueReportFormatter.Format(null!));
    }

    [Fact]
    public void OverdueReportBuilder_ReturnsOnlyOverdueLoansInCorrectOrder()
    {
        Loan first = CreateIssuedLoan(10, 10, 7);
        Loan second = CreateIssuedLoan(5, 20, 10);
        Loan notOverdue = CreateIssuedLoan(3, 30, 20);

        first.MarkOverdue(Start.AddDays(8));
        second.MarkOverdue(Start.AddDays(11));

        IReadOnlyList<DebtorLine> result = OverdueReportBuilder.Build(
            new[] { first, second, notOverdue },
            new DateOnly(2026, 1, 12),
            PricingCatalog.Default());

        Assert.Equal(2, result.Count);
        Assert.Equal(10, result[0].LoanId);
        Assert.Equal(5, result[1].LoanId);
    }

    [Fact]
    public void OverdueReportBuilder_NullArguments_ThrowArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(
            () => OverdueReportBuilder.Build(null!, new DateOnly(2026, 1, 10), PricingCatalog.Default()));

        Assert.Throws<ArgumentNullException>(
            () => OverdueReportBuilder.Build(Array.Empty<Loan>(), new DateOnly(2026, 1, 10), null!));
    }

    [Fact]
    public void ScenarioRunner_Success_LogsStartAndCompletion()
    {
        TestScenarioLog log = new();
        StringWriter output = new();
        ScenarioRunner runner = new(log, output);

        runner.Run("test", () => { });

        Assert.Equal(2, log.InformationCount);
        Assert.Equal(0, log.WarningCount);
        Assert.Equal(0, log.ErrorCount);
    }

    [Fact]
    public void ScenarioRunner_DomainException_LogsWarning()
    {
        TestScenarioLog log = new();
        StringWriter output = new();
        ScenarioRunner runner = new(log, output);

        runner.Run("test", () => throw new DomainRuleException("Domain error"));

        Assert.Equal(1, log.InformationCount);
        Assert.Equal(1, log.WarningCount);
        Assert.Contains("Domain error", output.ToString());
    }

    [Fact]
    public void ScenarioRunner_IoException_LogsError()
    {
        TestScenarioLog log = new();
        StringWriter output = new();
        ScenarioRunner runner = new(log, output);

        runner.Run("test", () => throw new IOException("File error"));

        Assert.Equal(1, log.InformationCount);
        Assert.Equal(1, log.ErrorCount);
        Assert.NotEmpty(output.ToString());
    }

    private static Loan CreateIssuedLoan(int id, int readerId, int days)
    {
        Loan loan = new(id, readerId, Start);
        loan.AddItem(new LoanItem($"ISBN-{id}", days, 2m));
        loan.Issue(Start);
        return loan;
    }

    private sealed class TestScenarioLog : IScenarioLog
    {
        public int InformationCount { get; private set; }

        public int WarningCount { get; private set; }

        public int ErrorCount { get; private set; }

        public void Information(string scenario, string message)
        {
            InformationCount++;
        }

        public void Warning(string scenario, string message)
        {
            WarningCount++;
        }

        public void LogError(string scenario, string message)
        {
            ErrorCount++;
        }
    }
}


