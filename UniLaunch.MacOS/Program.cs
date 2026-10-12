using UniLaunch.Core.Autostart;
using UniLaunch.Core.ExclusiveInstance;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.Core.Util;
using UniLaunch.MacOS.Autostart;
using UniLaunch.MacOS.Targets;
using UniLaunch.MacOS.ViewModel;
using UniLaunch.UI;
using UniLaunch.UI.ViewModels;

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

if (!CommandLineUtil.IsAutoStart())
{
    EntityViewModelRegistry.Instance
        .Register<AppFileTarget, AppFileTargetViewModel>();

    CommandLineUtil.RegisterAutoStart(new SharedListFileAutoStartRegistrationProvider());
    CommandLineUtil.PrintAppInfo();
    EditorUi.Run(Environment.GetCommandLineArgs());
    return 0;
}

// If running as autostart but there's no config file or no targets,
// fall back to showing the editor UI instead of exiting silently
// This ensures the app always shows a window when started without arguments
if (engine.Configuration == null || !engine.Configuration.Entries.Any())
{
    EntityViewModelRegistry.Instance
        .Register<AppFileTarget, AppFileTargetViewModel>();
    CommandLineUtil.PrintAppInfo();
    EditorUi.Run(Environment.GetCommandLineArgs());
    return 0;
}

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    result.Print();
}

return 0;
