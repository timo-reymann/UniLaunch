using Avalonia;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.Core.Util;
using UniLaunch.UI;

namespace UniLaunch.UIEditor;

/// <summary>
/// This is a simple wrapper to allow the Avalonia UI designer to do its thing.
/// </summary>
internal static class Program
{
    public static void Main(string[] args)
    {
        UniLaunchEngine
            .Instance
            .RegisterTarget<ExecutableTarget>()
            .RegisterRule<AlwaysRule>()
            .RegisterRule<TimeRule>()
            .RegisterRule<WeekDayRule>()
            .RegisterStorageProvider<YamlStorageProvider<UniLaunchConfiguration>>(true)
            .RegisterStorageProvider<JsonStorageProvider<UniLaunchConfiguration>>()
            .OverrideConfiguration(new UniLaunchConfiguration()
            {
                Targets =
                [
                    new ExecutableTarget
                    {
                        Executable = "/bin/bash",
                        Name = "Bash"
                    }
                ],
                RuleSets =
                [
                    new RuleSet
                    {
                        Name = "Ruleset #1",
                        Rules =
                        [
                            new AlwaysRule(),
                            new WeekDayRule
                            {
                                DaysOfWeekToRun =
                                [
                                    DayOfWeek.Friday, DayOfWeek.Saturday
                                ]
                            },
                            new TimeRule
                            {
                                StartRange = new(05, 30),
                                EndRange = new(10, 40),
                            }
                        ]
                    }
                ],
                Entries = [
                    new("Ruleset #1", "Bash")
                ]
            }, "",false);
        EditorUi.Run(args);
    }

    public static AppBuilder BuildAvaloniaApp() => EditorUi.BuildAvaloniaApp();
}