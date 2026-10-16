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
catch (ExclusiveInstanceAcquireFailedException)
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

// Run synchronously so the tray confirmation UI starts on the main thread
// (Avalonia/AppKit cannot run on a threadpool continuation after await).
foreach (var result in engine.WaitForAllTargetsToLaunch().GetAwaiter().GetResult())
{
    result.Print();
}

try
{
    EditorUi.RunTrayConfirmation(TimeSpan.FromSeconds(30));
}
catch (Exception)
{
    // Tray confirmation is best-effort and must never fail autostart.
}

return 0;