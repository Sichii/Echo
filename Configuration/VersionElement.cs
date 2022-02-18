using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Echo.Configuration
{
    public class VersionElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", DefaultValue = null, IsKey = true, IsRequired = true)]
        public string Name
        {
            get => (string) this[nameof(Name)];
            set => this[nameof(Name)] = value;
        }

        [ConfigurationProperty("ClientHash", DefaultValue = null, IsRequired = false)]
        public string ClientHash
        {
            get => (string)this[nameof(ClientHash)];
            set => this[nameof(ClientHash)] = value;
        }

        [ConfigurationProperty("VersionAddress", DefaultValue = null, IsRequired = true)]
        public string VersionAddress
        {
            get => (string) this[nameof(VersionAddress)];
            set => this [nameof(VersionAddress)] = value;
        }

        [ConfigurationProperty("VersionValue", DefaultValue = null, IsRequired = true)]
        public string VersionValue
        {
            get => (string) this[nameof(VersionValue)];
            set => this[nameof(VersionValue)] = value;
        }

        [ConfigurationProperty("CharacterNameAddress", DefaultValue = null, IsRequired = true)]
        public string CharacterNameAddress
        {
            get => (string) this[nameof(CharacterNameAddress)];
            set => this[nameof(CharacterNameAddress)] = value;
        }
        
        [ConfigurationProperty("SkipIntroAddress", DefaultValue = null, IsRequired = true)]
        public string SkipIntroAddress {
            get => (string)this[nameof(SkipIntroAddress)];
            set => this[nameof(SkipIntroAddress)] = value;
        }

        private uint? _versionAddressValue;
        private uint? _parsedVersionValue;
        private uint? _characterNameAddressValue;
        private uint? _skipIntroAddressValue;

        internal uint VersionAddressValue => _versionAddressValue ??= uint.Parse(VersionAddress, System.Globalization.NumberStyles.HexNumber);

        internal uint CharacterNameAddressValue => _characterNameAddressValue ??= uint.Parse(CharacterNameAddress, System.Globalization.NumberStyles.HexNumber);
        
        internal uint SkipIntroAddressValue => _skipIntroAddressValue ??= uint.Parse(SkipIntroAddress, System.Globalization.NumberStyles.HexNumber);

        internal uint ParsedVersionValue => _parsedVersionValue ??= uint.Parse(VersionValue);
    }
}