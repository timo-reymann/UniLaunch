using UniLaunch.Core.Autostart;
using UniLaunch.Core.ExclusiveInstance;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.Core.Util;
using UniLaunch.Linux.Autostart;
using UniLaunch.Linux.Desktop;
using UniLaunch.UI;

using var provider = new ExclusiveInstanceProvider();
try
{
    provider.Acquire();
}
catch (ExclusiveInstanceAcquireFailedException)
{
    return 16; // Device or resource busy
}

var engine = UniLaunchEngine.Instance
    .RegisterTarget<ExecutableTarget>()
    .RegisterRule<AlwaysRule>()
    .RegisterRule<TimeRule>()
    .RegisterRule<WeekDayRule>()
    .RegisterStorageProvider<YamlStorageProvider<UniLaunchConfiguration>>(true)
    .RegisterStorageProvider<JsonStorageProvider<UniLaunchConfiguration>>()
    .UseConfigFileLocator(new FileLocator(new List<string>
        {
            $"{XdgConfig.UserConfigFolder}/UniLaunch/config",
            "/etc/default/UniLaunch"
        }
    ))
    .LocateAndParseConfigFile();

if (!CommandLineUtil.IsAutoStart())
{
    CommandLineUtil.RegisterAutoStart(new DesktopFileStartRegistrationProvider());
    CommandLineUtil.PrintAppInfo();
    EditorUi.Run(Environment.GetCommandLineArgs());
    return 0;
}

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}

return 0;