using UniLaunch.Core.Meta;

namespace UniLaunch.UI;

public static class AppVersion
{
    public static string Current { get; } =
        new AppInfoProvider().VersionInfo.ProductVersion ?? "unknown";

    public static string MenuHeader => $"Version v{Current}";
}
