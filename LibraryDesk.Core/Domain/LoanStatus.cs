namespace LibraryDesk.Core.Domain;

/// <summary>Стан видачі книжок у її життєвому циклі.</summary>
public enum LoanStatus
{
    /// <summary>Книжки заброньовано.</summary>
    Reserved,

    /// <summary>Книжки видано читачеві.</summary>
    Issued,

    /// <summary>Книжки повернено.</summary>
    Returned,

    /// <summary>Строк повернення минув.</summary>
    Overdue,

    /// <summary>Бронь скасовано до видачі.</summary>
    Cancelled
}
