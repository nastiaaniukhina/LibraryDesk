using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Pricing;

namespace LibraryDesk.Core.Abstractions;

/// <summary>Каталог профілів розрахунку для категорій читачів.</summary>
public interface IPricingCatalog
{
    /// <summary>Повертає профіль для категорії читача.</summary>
    /// <param name="kind">Категорія читача.</param>
    /// <returns>Профіль розрахунку.</returns>
    PricingProfile For(ReaderKind kind);
}