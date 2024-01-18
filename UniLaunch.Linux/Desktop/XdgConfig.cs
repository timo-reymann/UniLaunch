using UniLaunch.Core.Util;

namespace UniLaunch.Linux.Desktop;

public static class XdgConfig
{
    public static string UserConfigFolder =>
        Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? $"{PathUtil.UserHome}/.config";
}