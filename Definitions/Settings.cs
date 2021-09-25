using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Echo.Definitions
{
    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue(@"C:\Program Files (x86)\KRU\Dark Ages\Darkages.exe")]
        public string DarkAgesPath
        {
            get => (string)this["DarkAgesPath"];
            set => this["DarkAgesPath"] = value;
        }

        public static Settings Default { get; } = (Settings)Synchronized(new Settings());
    }
}