namespace LibraryDesk.Core.Lending;

/// <summary>Стан формуляра видачі.</summary>
public enum LoanFormStatus
{
    /// <summary>Книжку заброньовано.</summary>
    Reserved,

    /// <summary>Книжку видано читачу.</summary>
    Issued,

    /// <summary>Книжку повернено.</summary>
    Returned,

    /// <summary>Видачу скасовано.</summary>
    Cancelled,
}
