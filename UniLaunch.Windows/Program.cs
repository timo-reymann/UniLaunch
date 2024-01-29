using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.Core.Util;
using UniLaunch.UI;
using UniLaunch.Windows.Autostart;

if (!CommandLineUtil.IsAutoStart())
{
    CommandLineUtil.RegisterAutoStart(new RegistryAutoStartRegistrationProvider());
    CommandLineUtil.PrintAppInfo();
    EditorUi.Main(Environment.GetCommandLineArgs());
    return 0;
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
            $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/UniLaunch/config"
        }
    ))
    .LocateAndParseConfigFile();

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}

return 0;