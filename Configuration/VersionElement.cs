using System.Configuration;
using System.Globalization;

namespace Echo.Configuration;

public class VersionElement : ConfigurationElement
{
    private uint? _characterNameAddressValue;
    private uint? _parsedVersionValue;
    private uint? _skipIntroAddressValue;

    private uint? _versionAddressValue;

    [ConfigurationProperty("Name", DefaultValue = null, IsKey = true, IsRequired = true)]
    public string Name
    {
        get => (string)this[nameof(Name)];
        set => this[nameof(Name)] = value;
    }

    [ConfigurationProperty("ClientHash", DefaultValue = "0")]
    public string ClientHash
    {
        get => (string)this[nameof(ClientHash)];
        set => this[nameof(ClientHash)] = value;
    }

    [ConfigurationProperty("VersionAddress", DefaultValue = null, IsRequired = true)]
    public string VersionAddress
    {
        get => (string)this[nameof(VersionAddress)];
        set => this[nameof(VersionAddress)] = value;
    }

    [ConfigurationProperty("VersionValue", DefaultValue = null, IsRequired = true)]
    public string VersionValue
    {
        get => (string)this[nameof(VersionValue)];
        set => this[nameof(VersionValue)] = value;
    }

    [ConfigurationProperty("CharacterNameAddress", DefaultValue = "0")]
    public string CharacterNameAddress
    {
        get => (string)this[nameof(CharacterNameAddress)];
        set => this[nameof(CharacterNameAddress)] = value;
    }

    [ConfigurationProperty("SkipIntroAddress", DefaultValue = "0")]
    public string SkipIntroAddress
    {
        get => (string)this[nameof(SkipIntroAddress)];
        set => this[nameof(SkipIntroAddress)] = value;
    }

    internal uint VersionAddressValue => _versionAddressValue ??= uint.Parse(VersionAddress, NumberStyles.HexNumber);

    internal uint CharacterNameAddressValue => _characterNameAddressValue ??= uint.Parse(CharacterNameAddress, NumberStyles.HexNumber);

    internal uint SkipIntroAddressValue => _skipIntroAddressValue ??= uint.Parse(SkipIntroAddress, NumberStyles.HexNumber);

    internal uint ParsedVersionValue => _parsedVersionValue ??= uint.Parse(VersionValue);
}