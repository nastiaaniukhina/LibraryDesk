using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Reports;

/// <summary>Формує перелік боржників без введення-виведення.</summary>
public static class OverdueReportBuilder
{
    /// <summary>Будує звіт на календарну дату.</summary>
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
                PricingProfile profile = pricing.For(entry.Loan.ReaderKind);
                decimal penalty = entry.Loan.Items.Sum(item =>
                    profile.Penalty.PenaltyOf(item, entry.Days, entry.Loan.ReaderKind == ReaderKind.Student));
                return new DebtorLine(entry.Loan.Id, entry.Loan.ReaderId, entry.Days, Money.Round(penalty));
            })
            .OrderByDescending(line => line.OverdueDays)
            .ThenBy(line => line.LoanId)
            .ToList();
    }
}