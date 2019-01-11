using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace NejeEngraverApp.Properties
{

[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.6.0.0"), CompilerGenerated]
internal sealed class Settings : ApplicationSettingsBase
{
    private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

    public static Settings Default
    {
        get
        {
            return Settings.defaultInstance;
        }
    }

    [DefaultSettingValue("Arial, 20.25pt"), UserScopedSetting, DebuggerNonUserCode]
    public Font Font
    {
        get
        {
            return (Font)this["Font"];
        }
        set
        {
            this["Font"] = value;
        }
    }

    [DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
    public int Language
    {
        get
        {
            return (int)this["Language"];
        }
        set
        {
            this["Language"] = value;
        }
    }

    [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
    public bool FirstRun_installdriver
    {
        get
        {
            return (bool)this["FirstRun_installdriver"];
        }
        set
        {
            this["FirstRun_installdriver"] = value;
        }
    }

    [DefaultSettingValue("True"), UserScopedSetting, DebuggerNonUserCode]
    public bool Upgrade_remind
    {
        get
        {
            return (bool)this["Upgrade_remind"];
        }
        set
        {
            this["Upgrade_remind"] = value;
        }
    }
}
}
