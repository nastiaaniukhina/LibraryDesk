namespace LibraryDesk.Core.Domain;

/// <summary>Підсумок прийняття повернення.</summary>
/// <param name="Total">Загальна вартість.</param>
/// <param name="Penalty">Нарахована пеня.</param>
/// <param name="OverdueDays">Кількість днів прострочення.</param>
public sealed record ReturnReceipt(decimal Total, decimal Penalty, int OverdueDays);