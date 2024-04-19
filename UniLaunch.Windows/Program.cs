using UniLaunch.Core.Autostart;
using UniLaunch.Core.ExclusiveInstance;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.Core.Util;
using UniLaunch.UI;
using UniLaunch.Windows.Autostart;

using var provider = new ExclusiveInstanceProvider();
try
{
    provider.Acquire();
}
catch (ExclusiveInstanceAcquireFailedException e)
{
    return 4; // ERROR_TOO_MANY_OPEN_FILES
}

var engine = UniLaunchEngine
    .Instance
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

if (!CommandLineUtil.IsAutoStart())
{
    CommandLineUtil.RegisterAutoStart(new RegistryAutoStartRegistrationProvider());
    CommandLineUtil.PrintAppInfo();
    EditorUi.Run(Environment.GetCommandLineArgs());
    return 0;
}

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}


return 0;