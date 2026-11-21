namespace LibraryDesk.Labs.Lending;

/// <summary>Р”Р°РЅС– С‡РёС‚Р°С‡Р°, С‰Рѕ РїРѕС‚СЂС–Р±РЅС– РґР»СЏ СЂРѕР·СЂР°С…СѓРЅРєСѓ.</summary>
/// <param name="Name">РџСЂС–Р·РІРёС‰Рµ С‡РёС‚Р°С‡Р°.</param>
/// <param name="Email">РџРѕС€С‚Р° С‡РёС‚Р°С‡Р°.</param>
/// <param name="IsRegular">Р§Рё С” С‡РёС‚Р°С‡ РїРѕСЃС‚С–Р№РЅРёРј.</param>
public sealed record ReaderInfo(string Name, string Email, bool IsRegular);
