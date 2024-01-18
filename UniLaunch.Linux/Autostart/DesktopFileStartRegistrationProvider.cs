using UniLaunch.Core.Autostart;
using UniLaunch.Linux.DesktopFile;

namespace UniLaunch.Linux.Autostart;

public class DesktopFileStartRegistrationProvider : AutoStartRegistrationProvider
{
    private static string DesktopFilePath => $"{XdgConfig.UserConfigFolder}/autostart/UniLaunch.desktop";
    
    public override void Register(List<string> arguments)
    {
        try
        {
            using var writer = new DesktopFileWriter(DesktopFilePath);
            writer.Write("Type", "Application");
            writer.Write("Exec", string.Join(" ", arguments));
            // TODO: Add + write icon
            // writer.Write("Icon","");
            writer.Write("X-GNOME-Autostart-enabled", "true");
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