using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.MacOS.Targets;

var engine = new UniLaunchEngine()
    .RegisterTarget<AppFileTarget>()
    .RegisterTarget<ExecutableTarget>()
    .RegisterRule<AlwaysRule>()
    .RegisterRule<TimeRule>()
    .RegisterRule<WeekDayRule>()
    .RegisterStorageProvider<YamlStorageProvider<UniLaunchConfiguration>>(true)
    .RegisterStorageProvider<JsonStorageProvider<UniLaunchConfiguration>>()
    .UseConfigFileLocator(
        new FileLocator(new List<string>
            {
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Library/Application Support/UniLaunch/config",
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config/uniLaunch",
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config/uniLaunch.macos"
            }
        )
    )
    .LocateAndParseConfigFile();

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}
