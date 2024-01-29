using System.Runtime.Versioning;
using Microsoft.Win32;
using UniLaunch.Core.Autostart;

namespace UniLaunch.Windows.Autostart;

[SupportedOSPlatform("windows")]
public class RegistryAutoStartRegistrationProvider : AutoStartRegistrationProvider
{
    private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string SubKeyName = "UniLaunch";
    
    private RegistryKey OpenKey() => 
        Registry.CurrentUser.OpenSubKey(RegistryKey, true)!;

    public override void Register(List<string> arguments)
    {
        var registryKey = OpenKey();
        registryKey.SetValue(SubKeyName,  $"{ExecutableFile} {string.Join(" ", arguments)}");
        registryKey.Flush();
    }

    public override void DeRegister(List<string> arguments)
    {
        var registryKey = OpenKey();
        registryKey.DeleteSubKey(SubKeyName);
        registryKey.Flush();
    }
}