namespace LibraryDesk.Core.Domain;

/// <summary>Стан видачі книжок у її життєвому циклі.</summary>
public enum LoanStatus
{
    Reserved = 0,
    Issued = 1,
    Returned = 2,
    Overdue = 3,
    Cancelled = 4,
}
