using System.Text;

namespace LibraryDesk.Core.Reports;

/// <summary>Формує текст простого звіту з готових рядків.</summary>
public sealed class ReportBuilder
{
    /// <summary>Gets заголовок звіту.</summary>
    public string Title { get; init; } = "Звіт";

    /// <summary>Будує текст звіту.</summary>
    /// <param name="rows">Рядки звіту.</param>
    /// <returns>Заголовок великими літерами та рядки, кожен з нового рядка.</returns>
    public string Build(IReadOnlyList<string> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        StringBuilder text = new();
        text.AppendLine(Title.ToUpperInvariant());
        foreach (string row in rows)
        {
            text.AppendLine(row);
        }

        return text.ToString();
    }
}
