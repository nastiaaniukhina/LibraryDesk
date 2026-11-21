namespace LibraryDesk.Core.Domain;

/// <summary>РќРµР·РјС–РЅРЅРёР№ С‡РёС‚Р°С‡ Р±С–Р±Р»С–РѕС‚РµРєРё.</summary>
public sealed record Reader
{
    /// <summary>Initializes a new instance of the <see cref="Reader"/> class.</summary>
    /// <param name="id">Р†РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ С‡РёС‚Р°С‡Р°, Р±С–Р»СЊС€РёР№ Р·Р° РЅСѓР»СЊ.</param>
    /// <param name="name">РџСЂС–Р·РІРёС‰Рµ С‚Р° С–Рј'СЏ, РЅРµ РїРѕСЂРѕР¶РЅС”.</param>
    /// <param name="email">РђРґСЂРµСЃР° РїРѕС€С‚Рё, С‰Рѕ РјС–СЃС‚РёС‚СЊ СЃРёРјРІРѕР» @.</param>
    public Reader(int id, string name, string email)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        if (!email.Contains('@'))
        {
            throw new ArgumentException("РџРѕС€С‚Р° РјР°С” РјС–СЃС‚РёС‚Рё СЃРёРјРІРѕР» @.", nameof(email));
        }

        Id = id;
        Name = name;
        Email = email;
    }

    /// <summary>Gets С–РґРµРЅС‚РёС„С–РєР°С‚РѕСЂ С‡РёС‚Р°С‡Р°.</summary>
    public int Id { get; }

    /// <summary>Gets РїСЂС–Р·РІРёС‰Рµ С‚Р° С–Рј'СЏ С‡РёС‚Р°С‡Р°.</summary>
    public string Name { get; }

    /// <summary>Gets Р°РґСЂРµСЃСѓ РїРѕС€С‚Рё С‡РёС‚Р°С‡Р°.</summary>
    public string Email { get; }

    /// <summary>Gets a value indicating whether С‡РёС‚Р°С‡ С” СЃС‚СѓРґРµРЅС‚РѕРј.</summary>
    public bool IsStudent { get; init; }
}
