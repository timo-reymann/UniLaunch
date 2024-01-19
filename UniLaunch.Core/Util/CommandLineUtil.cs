using UniLaunch.Core.Autostart;
using UniLaunch.Core.Meta;

namespace UniLaunch.Core.Util;

public static class CommandLineUtil
{
    public static bool IsAutoStart() => Environment.GetCommandLineArgs().Contains("--autostart");

    public static void RegisterAutoStart(AutoStartRegistrationProvider provider)
    {
        provider.Register(new List<string> { "--autostart" });
    }

    public static void PrintAppInfo()
    {
        var provider = new AppInfoProvider();
        Console.WriteLine($"Version: {provider.VersionInfo.ProductVersion}");
    }
}