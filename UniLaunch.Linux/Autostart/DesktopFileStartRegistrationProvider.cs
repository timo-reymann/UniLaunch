using System.Reflection;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Util;
using UniLaunch.Linux.Desktop;

namespace UniLaunch.Linux.Autostart;

public class DesktopFileStartRegistrationProvider : AutoStartRegistrationProvider
{
    private static string DesktopFilePath => $"{XdgConfig.UserConfigFolder}/autostart/UniLaunch.desktop";
    private static string IconPath => $"{PathUtil.UserHome}/.local/share/icons/UniLaunch.png";

    private Stream GetIconStream()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream("UniLaunch.Linux.Resources.icon.png")!;
    }

    private string GetExecutablePath()
    {
       var appImagePath = Environment.GetEnvironmentVariable("APPIMAGE");
       return appImagePath ?? ExecutableFile;
    }

    public override void Register(List<string> arguments)
    {
        try
        {
            using var desktopFileWriter = new DesktopFileWriter(DesktopFilePath);
            desktopFileWriter.Write("Name", "UniLaunch");
            desktopFileWriter.Write("Type", "Application");
            desktopFileWriter.Write("Exec", $"{GetExecutablePath()} {string.Join(" ", arguments)}");
            desktopFileWriter.Write("Icon", IconPath);
            desktopFileWriter.Write("X-GNOME-Autostart-enabled", "true");

            using var iconWriter = GetIconStream();
            using var file = File.Open(IconPath, FileMode.Create);
            iconWriter.CopyTo(file);
        }
        catch (Exception e)
        {
            throw new AutoStartRegistrationException("Failed to create desktop file for autostart", e);
        }
    }

    public override void DeRegister(List<string> arguments)
    {
        if (File.Exists(DesktopFilePath))
        {
            File.Delete(DesktopFilePath);
        }
        else
        {
            throw new AutoStartRegistrationException(
                "Could not remove registration for autostart as desktop file could not be found");
        }
    }
}