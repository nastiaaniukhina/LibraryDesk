using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Pricing;

namespace LibraryDesk.Core.Services;

/// <summary>Р С™Р С•Р С•РЎР‚Р Т‘Р С‘Р Р…РЎС“РЎвЂќ РЎРѓРЎвЂ Р ВµР Р…Р В°РЎР‚РЎвЂ“РЎвЂ” Р В¶Р С‘РЎвЂљРЎвЂљРЎвЂќР Р†Р С•Р С–Р С• РЎвЂ Р С‘Р С”Р В»РЎС“ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
public sealed class LoanLifecycleService
{
    private readonly ILoanRepository _repository;
    private readonly IPricingCatalog _pricing;
    private readonly INotifier _notifier;
    private readonly TimeProvider _time;

    /// <summary>Р РЋРЎвЂљР Р†Р С•РЎР‚РЎР‹РЎвЂќ РЎРѓР ВµРЎР‚Р Р†РЎвЂ“РЎРѓ Р В¶Р С‘РЎвЂљРЎвЂљРЎвЂќР Р†Р С•Р С–Р С• РЎвЂ Р С‘Р С”Р В»РЎС“.</summary>
    public LoanLifecycleService(ILoanRepository repository, IPricingCatalog pricing, INotifier notifier, TimeProvider time)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        _time = time ?? throw new ArgumentNullException(nameof(time));
    }

    /// <summary>Р В Р ВµРЎвЂќРЎРѓРЎвЂљРЎР‚РЎС“РЎвЂќ Р Р…Р С•Р Р†РЎС“ Р В±РЎР‚Р С•Р Р…РЎРЉ.</summary>
    public void Place(Loan loan, Reader reader)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentNullException.ThrowIfNull(reader);
        if (loan.ReaderId != reader.Id)
        {
            throw new ArgumentException("Р вЂ™Р С‘Р Т‘Р В°РЎвЂЎР В° Р Р…Р В°Р В»Р ВµР В¶Р С‘РЎвЂљРЎРЉ РЎвЂ“Р Р…РЎв‚¬Р С•Р СРЎС“ РЎвЂЎР С‘РЎвЂљР В°РЎвЂЎР ВµР Р†РЎвЂ“.", nameof(reader));
        }

        if (loan.Items.Count == 0)
        {
            throw new DomainRuleException("Р вЂРЎР‚Р С•Р Р…РЎР‹Р Р†Р В°Р Р…Р Р…РЎРЏ Р СР В°РЎвЂќ Р СРЎвЂ“РЎРѓРЎвЂљР С‘РЎвЂљР С‘ РЎвЂ¦Р С•РЎвЂЎР В° Р В± Р С•Р Т‘Р Р…РЎС“ Р С”Р Р…Р С‘Р В¶Р С”РЎС“.");
        }

        _repository.Add(loan);
        _repository.Save();
        _notifier.Notify(reader.Email, $"Р РЋРЎвЂљР Р†Р С•РЎР‚Р ВµР Р…Р С• Р В±РЎР‚Р С•Р Р…РЎР‹Р Р†Р В°Р Р…Р Р…РЎРЏ #{loan.Id}.");
    }

    /// <summary>Р вЂ™Р С‘Р Т‘Р В°РЎвЂќ Р В·Р В°Р В±РЎР‚Р С•Р Р…РЎРЉР С•Р Р†Р В°Р Р…РЎвЂ“ Р С”Р Р…Р С‘Р В¶Р С”Р С‘.</summary>
    public void Issue(int loanId)
    {
        Loan loan = Find(loanId);
        loan.Issue(_time.GetUtcNow());
        _repository.Save();
        _notifier.Notify(loan.ReaderId.ToString(), $"Р вЂ™Р С‘Р Т‘Р В°РЎвЂЎРЎС“ #{loan.Id} Р С•РЎвЂћР С•РЎР‚Р СР В»Р ВµР Р…Р С•.");
    }

    /// <summary>Р СџРЎР‚Р С‘Р в„–Р СР В°РЎвЂќ Р С—Р С•Р Р†Р ВµРЎР‚Р Р…Р ВµР Р…Р Р…РЎРЏ РЎвЂљР В° Р С•Р В±РЎвЂЎР С‘РЎРѓР В»РЎР‹РЎвЂќ Р С—Р ВµР Р…РЎР‹.</summary>
    public ReturnReceipt ReturnLoan(int loanId)
    {
        Loan loan = Find(loanId);
        DateOnly today = DateOnly.FromDateTime(_time.GetUtcNow().UtcDateTime);
        int days = loan.OverdueDaysAt(today);
        PricingProfile profile = _pricing.GetFor(loan.ReaderKind);
        decimal penalty = loan.Items.Sum(item => profile.Penalty.PenaltyOf(item, days, loan.ReaderKind == ReaderKind.Student));
        decimal total = Money.Round(loan.Items.Sum(profile.Fee.PriceOf));
        loan.Return(_time.GetUtcNow());
        _repository.Save();
        _notifier.Notify(loan.ReaderId.ToString(), $"Р СџР С•Р Р†Р ВµРЎР‚Р Р…Р ВµР Р…Р Р…РЎРЏ #{loan.Id} Р С—РЎР‚Р С‘Р в„–Р Р…РЎРЏРЎвЂљР С•.");
        return new ReturnReceipt(total, Money.Round(penalty), days);
    }

    /// <summary>Р РЋР С”Р В°РЎРѓР С•Р Р†РЎС“РЎвЂќ Р В±РЎР‚Р С•Р Р…РЎР‹Р Р†Р В°Р Р…Р Р…РЎРЏ.</summary>
    public void Cancel(int loanId)
    {
        Loan loan = Find(loanId);
        loan.Cancel();
        _repository.Save();
        _notifier.Notify(loan.ReaderId.ToString(), $"Р вЂРЎР‚Р С•Р Р…РЎР‹Р Р†Р В°Р Р…Р Р…РЎРЏ #{loan.Id} РЎРѓР С”Р В°РЎРѓР С•Р Р†Р В°Р Р…Р С•.");
    }

    /// <summary>Р С›Р В±РЎвЂЎР С‘РЎРѓР В»РЎР‹РЎвЂќ Р В·Р В°Р С–Р В°Р В»РЎРЉР Р…РЎС“ Р Р†Р В°РЎР‚РЎвЂљРЎвЂ“РЎРѓРЎвЂљРЎРЉ Р Р†Р С‘Р Т‘Р В°РЎвЂЎРЎвЂ“.</summary>
    public decimal TotalOf(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        return Money.Round(loan.Items.Sum(_pricing.GetFor(loan.ReaderKind).Fee.PriceOf));
    }

    private Loan Find(int id)
    {
        return _repository.GetById(id) ?? throw new LoanNotFoundException($"Р вЂ™Р С‘Р Т‘Р В°РЎвЂЎРЎС“ {id} Р Р…Р Вµ Р В·Р Р…Р В°Р в„–Р Т‘Р ВµР Р…Р С•.");
    }
}
