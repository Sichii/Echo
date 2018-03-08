using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DAWindower
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        public static Settings Default { get; } = (Settings)Synchronized(new Settings());

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue(@"C:\Program Files (x86)\KRU\Dark Ages\Darkages.exe")]
        public string DarkAgesPath
        {
            get => (string)this["DarkAgesPath"];
            set => this["DarkAgesPath"] = value;
        }
    }
}
