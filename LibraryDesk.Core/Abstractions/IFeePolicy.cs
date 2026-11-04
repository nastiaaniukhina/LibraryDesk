using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Abstractions;

/// <summary>Політика розрахунку плати за позицію видачі.</summary>
public interface IFeePolicy
{
    /// <summary>Рахує плату за одну позицію.</summary>
    /// <param name="item">Позиція видачі.</param>
    /// <returns>Плата за позицію, грн.</returns>
    decimal PriceOf(LoanItem item);
}
