namespace LibraryDesk.Labs.Lending;

/// <summary>Р РµР·СѓР»СЊС‚Р°С‚ СЂРѕР·СЂР°С…СѓРЅРєСѓ РїС–РґСЃСѓРјРєСѓ РІРёРґР°С‡С–.</summary>
/// <param name="Total">РџС–РґСЃСѓРјРѕРє РґРѕ СЃРїР»Р°С‚Рё.</param>
/// <param name="Status">РЎС‚Р°РЅ РІРёРґР°С‡С– РїС–СЃР»СЏ СЂРѕР·СЂР°С…СѓРЅРєСѓ.</param>
public sealed record LoanTotalResult(decimal Total, LoanFormStatus Status);
