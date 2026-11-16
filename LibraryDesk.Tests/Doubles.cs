using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Tests;

internal sealed class FakeLoanRepository : ILoanRepository
{
    private readonly Dictionary<int, Loan> _items = new();

    public int AddCalls { get; private set; }

    public void Add(Loan loan)
    {
        AddCalls++;
        _items[loan.Id] = loan;
    }

    public Loan? GetById(int id) => _items.GetValueOrDefault(id);

    public IReadOnlyList<Loan> GetAll() => _items.Values.ToList();
}

internal sealed class StubFeePolicy : IFeePolicy
{
    private readonly decimal _price;

    public StubFeePolicy(decimal price) => _price = price;

    public decimal PriceOf(LoanItem item) => _price;
}

internal sealed class SpyNotifier : INotifier
{
    public List<(string To, string Message)> Sent { get; } = new();

    public void Notify(string to, string message) => Sent.Add((to, message));
}
