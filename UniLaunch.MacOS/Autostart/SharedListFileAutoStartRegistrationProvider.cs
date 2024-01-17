using System.Diagnostics;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Util;
using UniLaunch.MacOS.Plist;

namespace UniLaunch.MacOS.Autostart;

public class SharedListFileAutoStartRegistrationProvider : IAutoStartRegistrationProvider
{
    private const string GroupId = "de.timo_reymann.unilaunch";

    private string LaunchPListFile => $"{PathUtil.UserHome}/Library/LaunchAgents/{GroupId}";
    private string AppFile => Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!;

    public void Register( List<string> arguments)
    {
        using var writer = new PlistDictWriter(LaunchPListFile);
        writer.WriteString("Label", GroupId);
        writer.WriteArray("ProgramArguments", new[] { AppFile }.Concat(arguments).ToArray());
        writer.WriteBool("RunAtLoad", true);
        writer.WriteString("StandardOutPath", $"{PathUtil.UserHome}/Library/Logs/UniLaunch/launchd-stdout.log");
        writer.WriteString("StandardErrorPath", $"{PathUtil.UserHome}/Library/Logs/UniLaunch/launchd-stderr.log");
    }

    public void Register()
    {
        Register(new List<string>());
    }

    public void DeRegister(List<string> arguments)
    {
        if (File.Exists(LaunchPListFile))
        {
            File.Delete(LaunchPListFile);
        }
    }

    public void DeRegister()
    {
        DeRegister(new List<string>());
    }
}