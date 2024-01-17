using System.Diagnostics;using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;

var watch = new Stopwatch();
watch.Start();

var config = new UniLaunchConfiguration
{
    Targets =
    {
        new AppFileTarget
        {
            Name = "iTerm",
            Path = "/Applications/iTerm.app"
        },
        new ExecutableTarget
        {
            Name = "Slack",
            Executable = "/usr/bin/op1en"
        }
    },
    RuleSets =
    {
        new RuleSet
        {
            Name = "Default",
            Rules = new List<Rule>
            {
                new WeekDayRule
                {
                    DaysOfWeekToRun = new[] { DayOfWeek.Saturday }
                },
                new AlwaysRule(),
                new TimeRule
                {
                    StartRange = new(08, 20),
                    EndRange = new(23, 30)
                }
            }
        }
    },
    Entries =
    {
        new AutoStartEntry("Default", "iTerm"),
        new AutoStartEntry("Default", "Slack")
    },
};

var engine = new UniLaunchEngine()
    // Register all activated targets
    .RegisterTarget<AppFileTarget>()
    .RegisterTarget<ExecutableTarget>()
    // Register builtin rules
    .RegisterRule<AlwaysRule>()
    .RegisterRule<TimeRule>()
    .RegisterRule<WeekDayRule>()
    // Storage provider registration
    .RegisterStorageProvider<YamlStorageProvider<UniLaunchConfiguration>>(true)
    .RegisterStorageProvider<JsonStorageProvider<UniLaunchConfiguration>>()
    // Setup possible configuration locations
    .UseConfigFileLocator(new FileLocator(new List<string>()
        {
            "uniLaunchConfig",
            $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/uniLaunchConfig"
        }, "uniLaunchConfig")
    )
    // Try locate and parse config file
    .LocateAndParseConfigFile();
    // Override config on disk
    //.OverrideConfiguration(config);

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    Console.Write(result.Target.Name + " => " + result.Status);
    if (result.Errors == null)
    {
        continue;
    }

    Console.Write("( ");
    foreach (var resultError in result.Errors)
    {
        Console.Write(resultError.Details?.Trim() ?? "N/A");
    }

    Console.Write(" )");

    Console.WriteLine();
}

watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
