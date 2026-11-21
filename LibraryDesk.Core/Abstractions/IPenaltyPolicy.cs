using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Abstractions;

/// <summary>Політика розрахунку пені за прострочення.</summary>
public interface IPenaltyPolicy
{
    /// <summary>Розраховує пеню за одну позицію видачі.</summary>
    /// <param name="item">Позиція видачі.</param>
    /// <param name="overdueDays">Кількість днів прострочення.</param>
    /// <param name="isStudent">Чи є читач студентом.</param>
    /// <returns>Сума пені, обмежена вартістю позиції.</returns>
    decimal PenaltyOf(LoanItem item, int overdueDays, bool isStudent);
}
