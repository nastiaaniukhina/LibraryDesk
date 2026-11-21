using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Services;

/// <summary>Координує сценарії життєвого циклу видачі.</summary>
public sealed class LoanLifecycleService
{
    private readonly ILoanRepository _repository;
    private readonly IPricingCatalog _pricing;
    private readonly INotifier _notifier;
    private readonly TimeProvider _time;

    /// <summary>Створює сервіс життєвого циклу.</summary>
    public LoanLifecycleService(ILoanRepository repository, IPricingCatalog pricing, INotifier notifier, TimeProvider time)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _pricing = pricing ?? throw new ArgumentNullException(nameof(pricing));
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        _time = time ?? throw new ArgumentNullException(nameof(time));
    }

    /// <summary>Реєструє нову бронь.</summary>
    public void Place(Loan loan, Reader reader)
    {
        ArgumentNullException.ThrowIfNull(loan);
        ArgumentNullException.ThrowIfNull(reader);
        if (loan.ReaderId != reader.Id) throw new ArgumentException("Видача належить іншому читачеві.", nameof(reader));
        if (loan.Items.Count == 0) throw new DomainRuleException("Бронювання має містити хоча б одну книжку.");
        _repository.Add(loan);
        _repository.Save();
        _notifier.Notify(reader.Email, $"Створено бронювання #{loan.Id}.");
    }

    /// <summary>Видає заброньовані книжки.</summary>
    public void Issue(int loanId)
    {
        Loan loan = Find(loanId);
        loan.Issue(_time.GetUtcNow());
        _repository.Save();
        _notifier.Notify(loan.ReaderId.ToString(), $"Видачу #{loan.Id} оформлено.");
    }

    /// <summary>Приймає повернення та обчислює пеню.</summary>
    public ReturnReceipt ReturnLoan(int loanId)
    {
        Loan loan = Find(loanId);
        DateOnly today = DateOnly.FromDateTime(_time.GetUtcNow().UtcDateTime);
        int days = loan.OverdueDaysAt(today);
        PricingProfile profile = _pricing.For(loan.ReaderKind);
        decimal penalty = loan.Items.Sum(item => profile.Penalty.PenaltyOf(item, days, loan.ReaderKind == ReaderKind.Student));
        decimal total = Money.Round(loan.Items.Sum(profile.Fee.PriceOf));
        loan.Return(_time.GetUtcNow());
        _repository.Save();
        _notifier.Notify(loan.ReaderId.ToString(), $"Повернення #{loan.Id} прийнято.");
        return new ReturnReceipt(total, Money.Round(penalty), days);
    }

    /// <summary>Скасовує бронювання.</summary>
    public void Cancel(int loanId)
    {
        Loan loan = Find(loanId);
        loan.Cancel();
        _repository.Save();
        _notifier.Notify(loan.ReaderId.ToString(), $"Бронювання #{loan.Id} скасовано.");
    }

    /// <summary>Обчислює загальну вартість видачі.</summary>
    public decimal TotalOf(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        return Money.Round(loan.Items.Sum(_pricing.For(loan.ReaderKind).Fee.PriceOf));
    }

    private Loan Find(int id)
    {
        return _repository.GetById(id) ?? throw new LoanNotFoundException($"Видачу {id} не знайдено.");
    }
}