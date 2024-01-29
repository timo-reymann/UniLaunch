using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.Core.Util;
using UniLaunch.Linux.Autostart;
using UniLaunch.Linux.Desktop;
using UniLaunch.UI;

if (!CommandLineUtil.IsAutoStart())
{
    CommandLineUtil.RegisterAutoStart(new DesktopFileStartRegistrationProvider());
    CommandLineUtil.PrintAppInfo();
    EditorUi.Main(Environment.GetCommandLineArgs());
    return;
}

var engine = new UniLaunchEngine()
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

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}