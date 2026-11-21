using LibraryDesk.Core.Domain;

namespace LibraryDesk.Core.Abstractions;

/// <summary>РЎС…РѕРІРёС‰Рµ РІРёРґР°С‡.</summary>
public interface ILoanRepository
{
    /// <summary>Р”РѕРґР°С” РІРёРґР°С‡Сѓ РґРѕ СЃС…РѕРІРёС‰Р°.</summary>
    /// <param name="loan">Р’РёРґР°С‡Р°.</param>
    void Add(Loan loan);

    /// <summary>РЁСѓРєР°С” РІРёРґР°С‡Сѓ Р·Р° С–РґРµРЅС‚РёС„С–РєР°С‚РѕСЂРѕРј.</summary>
    /// <param name="id">Р†РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ РІРёРґР°С‡С–.</param>
    /// <returns>Р’РёРґР°С‡Р° Р°Р±Рѕ null, СЏРєС‰Рѕ С—С— РЅРµРјР°С”.</returns>
    Loan? GetById(int id);

    /// <summary>РџРѕРІРµСЂС‚Р°С” РІСЃС– РІРёРґР°С‡С–.</summary>
    /// <returns>РљРѕРїС–СЏ СЃРїРёСЃРєСѓ РІРёРґР°С‡.</returns>
    IReadOnlyList<Loan> GetAll();

    /// <summary>Р—Р±РµСЂС–РіР°С” РїРѕС‚РѕС‡РЅС– Р·РјС–РЅРё.</summary>
    void Save();
}
