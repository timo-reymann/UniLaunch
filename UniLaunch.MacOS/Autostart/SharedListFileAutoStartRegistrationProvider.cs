using System.Diagnostics;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Util;
using UniLaunch.MacOS.Plist;

namespace UniLaunch.MacOS.Autostart;

public class SharedListFileAutoStartRegistrationProvider : AutoStartRegistrationProvider
{
    private const string GroupId = "de.timo_reymann.unilaunch";

    private string LaunchPListFile => $"{PathUtil.UserHome}/Library/LaunchAgents/{GroupId}.plist";
    private string AppFile => Path.GetDirectoryName(ExecutableFile)! + "/UniLaunch";

    public override void Register(List<string> arguments)
    {
        try
        {
            using var writer = new PlistDictWriter(LaunchPListFile);
            writer.WriteString("Label", GroupId);
            writer.WriteArray("ProgramArguments", new[] { AppFile }.Concat(arguments).ToArray());
            writer.WriteBool("RunAtLoad", true);
            writer.WriteBool("AbandonProcessGroup", true);
            writer.WriteString("StandardOutPath", $"{PathUtil.UserHome}/Library/Logs/UniLaunch/launchd-stdout.log");
            writer.WriteString("StandardErrorPath", $"{PathUtil.UserHome}/Library/Logs/UniLaunch/launchd-stderr.log");
        }
        catch (Exception e)
        {
            throw new AutoStartRegistrationException("Failed to write autostart entry", e);
        }
    }

    public override void DeRegister(List<string> arguments)
    {
        if (File.Exists(LaunchPListFile))
        {
            File.Delete(LaunchPListFile);
        }
        else
        {
            throw new AutoStartRegistrationException(
                "Could not remove registration for autostart as item could not be found");
        }
    }
}