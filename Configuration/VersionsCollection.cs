using System.Configuration;

namespace Echo.Configuration;

[ConfigurationCollection(typeof(VersionsCollection), AddItemName = "Version")]
public class VersionsCollection : ConfigurationElementCollection
{
    public new VersionElement this[string name]
    {
        get => (VersionElement)BaseGet(name);
        set => BaseAdd(value, true);
    }

    protected override ConfigurationElement CreateNewElement() => new VersionElement();

    protected override object GetElementKey(ConfigurationElement element) => ((VersionElement)element).Name;

    public void Add(VersionElement versionElement) => BaseAdd(versionElement, true);
    public void Clear() => BaseClear();
    public void Remove(string name) => BaseRemove(name);
}