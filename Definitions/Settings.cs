using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Echo.Definitions;

[CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
    [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue(@"C:\Program Files (x86)\KRU\Dark Ages\Darkages.exe")]
    public string DarkAgesPath
    {
        get => (string)this["DarkAgesPath"];
        set => this["DarkAgesPath"] = value;
    }

    [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue(null)]
    public string PrimaryDeviceName
    {
        get => (string)this["PrimaryDeviceName"];
        set => this["PrimaryDeviceName"] = value;
    }

    [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("640x480 (Small)")]
    public string SizeSelection
    {
        get => (string)this["SizeSelection"];
        set => this["SizeSelection"] = value;
    }

    [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
    public bool UseDawnd
    {
        get => (bool)this["UseDawnd"];
        set => this["UseDawnd"] = value;
    }

    [UserScopedSetting, DebuggerNonUserCode, DefaultSettingValue("True")]
    public bool UseDDrawCompat
    {
        get => (bool)this["UseDDrawCompat"];
        set => this["UseDDrawCompat"] = value;
    }

    public static Settings Instance { get; } = (Settings)Synchronized(new Settings());
    public static Settings Default { get; } = new();
}