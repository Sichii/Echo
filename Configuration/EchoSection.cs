using System.Configuration;

namespace Echo.Configuration;

public class EchoSection : ConfigurationSection
{
    [ConfigurationProperty("Versions")]
    public VersionsCollection VersionsCollection
    {
        get => (VersionsCollection)this["Versions"];
        set => this["Versions"] = value;
    }
}