namespace LibraryDesk.Core.Abstractions;

/// <summary>РЎРїРѕРІС–С‰СѓРІР°С‡ РїСЂРѕ РїРѕРґС–С— РІРёРґР°С‡С–.</summary>
public interface INotifier
{
    /// <summary>РќР°РґСЃРёР»Р°С” РїРѕРІС–РґРѕРјР»РµРЅРЅСЏ РѕС‚СЂРёРјСѓРІР°С‡Сѓ.</summary>
    /// <param name="recipient">РђРґСЂРµСЃР° РѕС‚СЂРёРјСѓРІР°С‡Р°.</param>
    /// <param name="message">РўРµРєСЃС‚ РїРѕРІС–РґРѕРјР»РµРЅРЅСЏ.</param>
    void Notify(string recipient, string message);
}
