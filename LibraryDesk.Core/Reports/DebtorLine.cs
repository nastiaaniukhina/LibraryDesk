namespace LibraryDesk.Core.Reports;

/// <summary>Рядок звіту про боржника.</summary>
public sealed record DebtorLine(int LoanId, int ReaderId, int OverdueDays, decimal Penalty);