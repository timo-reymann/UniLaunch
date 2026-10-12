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

// If running as autostart but there's no config file or no targets,
// fall back to showing the editor UI instead of exiting silently
// This ensures the app always shows a window when started without arguments
// Only fallback on missing config (not load failure) to avoid showing empty UI on corrupt file
if (engine.Configuration == null || engine.ConfigLoadFailed || !engine.Configuration.Entries.Any())
{
    CommandLineUtil.PrintAppInfo();
    EditorUi.Run(Environment.GetCommandLineArgs());
    return 0;
}

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}

return 0;