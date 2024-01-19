using System.Diagnostics;
using System.Reflection;

namespace UniLaunch.Core.Meta;

public class AppInfoProvider
{
    public AppInfoProvider()
    {
        var executingAssembly = Assembly.GetEntryAssembly()!;
        VersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
    }

    public FileVersionInfo VersionInfo { get; private set; }
}