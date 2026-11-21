namespace LibraryDesk.Core.Reports;

/// <summary>Р СЏРґРѕРє Р·РІС–С‚Сѓ РїСЂРѕ Р±РѕСЂР¶РЅРёРєР°.</summary>
public sealed record DebtorLine(int LoanId, int ReaderId, int OverdueDays, decimal Penalty);
