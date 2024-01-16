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
            Name = "Always",
            Rules = new List<Rule>
            {
                new WeekDayRule()
                {
                    DaysOfWeekToRun = new [] { DayOfWeek.Saturday }
                },
                new AlwaysRule(),
                new TimeRule()
                {
                    StartRange = new(08, 20),
                    EndRange = new(23, 30)
                }
            }
        }
    },
    Entries =
    {
        new AutoStartEntry("Always", "iTerm"),
        new AutoStartEntry("Always", "Slack")
    },
};

var engine = new AutoStartEngine()
    // Register all activated targerts
    .RegisterTarget<AppFileTarget>()
    .RegisterTarget<ExecutableTarget>()
    // Register builtin rules
    .RegisterRule<AlwaysRule>()
    .RegisterRule<TimeRule>()
    .RegisterRule<WeekDayRule>()
    // Load configuration
    .ApplyConfiguration(config);

foreach (var result in await engine.WaitForAllTargetsToLaunch())
{
    Console.Write(result.Target.Name + " => " + result.Status);
    if (result.Errors != null)
    {
        Console.Write("( ");
        foreach (var resultError in result.Errors)
        {
            Console.Write(resultError.Details?.Trim() ?? "N/A");
        }

        Console.Write(" )");
    }

    Console.WriteLine();
}

JsonStorageProvider<AutostartConfiguration> storageProvider = new();
storageProvider.Persist("autoStartConfig", config);
config = storageProvider.Load("autoStartConfig");
Console.WriteLine(config);
