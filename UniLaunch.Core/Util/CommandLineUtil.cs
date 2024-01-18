using UniLaunch.Core.Autostart;

namespace UniLaunch.Core.Util;

public static class CommandLineUtil
{
    public static bool IsAutoStart() => Environment.GetCommandLineArgs().Contains("--autostart");

    public static void RegisterAutoStart(AutoStartRegistrationProvider provider)
    {
        provider.Register(new List<string> { "--autostart" });
    }
}