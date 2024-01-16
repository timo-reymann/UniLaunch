using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;

var config = new AutostartConfiguration()
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
                    DaysOfWeekToRun = new [] { DayOfWeek.Saturday }
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

var engine = new AutoStartEngine()
    // Register all activated targets
    .RegisterTarget<AppFileTarget>()
    .RegisterTarget<ExecutableTarget>()
    // Register builtin rules
    .RegisterRule<AlwaysRule>()
    .RegisterRule<TimeRule>()
    .RegisterRule<WeekDayRule>()
    // Storage provider registration
    .RegisterStorageProvider<YamlStorageProvider<AutostartConfiguration>>(true)
    .RegisterStorageProvider<JsonStorageProvider<AutostartConfiguration>>()
    // Load configuration
    .ApplyConfiguration(config);

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

var storageProvider = engine.DefaultStorageProvider;
storageProvider.Persist("autoStartConfig", config);
storageProvider.Load("autoStartConfig");
