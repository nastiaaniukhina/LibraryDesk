namespace LibraryDesk.Labs.Lending;

/// <summary>Р’С…С–РґРЅС– РґР°РЅС– СЂРѕР·СЂР°С…СѓРЅРєСѓ РїС–РґСЃСѓРјРєСѓ РІРёРґР°С‡С–.</summary>
/// <param name="LoanId">РќРѕРјРµСЂ РІРёРґР°С‡С–.</param>
/// <param name="Reader">Р”Р°РЅС– С‡РёС‚Р°С‡Р°.</param>
/// <param name="Lines">Р СЏРґРєРё РІРёРґР°С‡С–.</param>
/// <param name="CreatedAt">Р”Р°С‚Р° СЃС‚РІРѕСЂРµРЅРЅСЏ РІРёРґР°С‡С–.</param>
/// <param name="CouponCode">РљРѕРґ РєСѓРїРѕРЅР° Р°Р±Рѕ null.</param>
/// <param name="Status">РџРѕС‚РѕС‡РЅРёР№ СЃС‚Р°РЅ РІРёРґР°С‡С–.</param>
/// <param name="DeliveryPrice">Р’Р°СЂС‚С–СЃС‚СЊ РґРѕСЃС‚Р°РІРєРё.</param>
public sealed record LoanTotalRequest(
    int LoanId,
    ReaderInfo Reader,
    IReadOnlyList<LoanLine> Lines,
    DateOnly CreatedAt,
    string? CouponCode,
    LoanFormStatus Status,
    decimal DeliveryPrice);
