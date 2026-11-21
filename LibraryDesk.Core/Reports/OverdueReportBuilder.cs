using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Pricing;

namespace LibraryDesk.Core.Reports;

/// <summary>Р В¤Р С•РЎР‚Р СРЎС“РЎвЂќ Р С—Р ВµРЎР‚Р ВµР В»РЎвЂ“Р С” Р В±Р С•РЎР‚Р В¶Р Р…Р С‘Р С”РЎвЂ“Р Р† Р В±Р ВµР В· Р Р†Р Р†Р ВµР Т‘Р ВµР Р…Р Р…РЎРЏ-Р Р†Р С‘Р Р†Р ВµР Т‘Р ВµР Р…Р Р…РЎРЏ.</summary>
public static class OverdueReportBuilder
{
    /// <summary>Р вЂРЎС“Р Т‘РЎС“РЎвЂќ Р В·Р Р†РЎвЂ“РЎвЂљ Р Р…Р В° Р С”Р В°Р В»Р ВµР Р…Р Т‘Р В°РЎР‚Р Р…РЎС“ Р Т‘Р В°РЎвЂљРЎС“.</summary>
    public static IReadOnlyList<DebtorLine> Build(IReadOnlyList<Loan> loans, DateOnly asOf, IPricingCatalog pricing)
    {
        ArgumentNullException.ThrowIfNull(loans);
        ArgumentNullException.ThrowIfNull(pricing);
        return loans
            .Where(loan => loan.Status is LoanStatus.Issued or LoanStatus.Overdue)
            .Select(loan => (Loan: loan, Days: loan.OverdueDaysAt(asOf)))
            .Where(entry => entry.Days > 0)
            .Select(entry =>
            {
                PricingProfile profile = pricing.GetFor(entry.Loan.ReaderKind);
                decimal penalty = entry.Loan.Items.Sum(item =>
                    profile.Penalty.PenaltyOf(item, entry.Days, entry.Loan.ReaderKind == ReaderKind.Student));
                return new DebtorLine(entry.Loan.Id, entry.Loan.ReaderId, entry.Days, Money.Round(penalty));
            })
            .OrderByDescending(line => line.OverdueDays)
            .ThenBy(line => line.LoanId)
            .ToList();
    }
}
