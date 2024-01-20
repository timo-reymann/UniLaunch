using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.Core.Util;
using UniLaunch.MacOS.Autostart;
using UniLaunch.MacOS.Targets;

if (!CommandLineUtil.IsAutoStart())
{
    CommandLineUtil.RegisterAutoStart(new SharedListFileAutoStartRegistrationProvider());
    CommandLineUtil.PrintAppInfo();
}

var engine = new UniLaunchEngine()
    .RegisterTarget<AppFileTarget>()
    .RegisterTarget<ExecutableTarget>()
    .RegisterRule<AlwaysRule>()
    .RegisterRule<TimeRule>()
    .RegisterRule<WeekDayRule>()
    .RegisterStorageProvider<YamlStorageProvider<UniLaunchConfiguration>>(true)
    .RegisterStorageProvider<JsonStorageProvider<UniLaunchConfiguration>>()
    .UseConfigFileLocator(new FileLocator(new List<string>
        {
            $"{PathUtil.UserHome}/Library/Application Support/UniLaunch/config",
            $"{PathUtil.UserHome}/.config/uniLaunch",
            $"{PathUtil.UserHome}/.config/uniLaunch.macos"
        }
    ))
    .LocateAndParseConfigFile();

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}