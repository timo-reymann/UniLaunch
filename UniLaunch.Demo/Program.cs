using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;

var config = new AutostartConfiguration()
{
    Targets =
    {
        new ExecutableTarget
        {
            Name = "iTerm",
            Executable = "/Applications/iTerm.app/Contents/MacOS/iTerm2"
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
                /* new WeekDayRule()
                {
                    DaysToRun = new [] { DayOfWeek.Saturday }
                }, */
                new AlwaysRule(),
                /*new TimeRule()
                {
                    StartRange = new(19, 20),
                    EndRange = new(23, 30)
                }*/
            }
        }
    },
    Entries =
    {
        new AutoStartEntry("Always", "iTerm"),
        new AutoStartEntry("Always", "Slack")
    },
};

var engine = new AutoStartEngine(config);
var results = await engine.WaitForAllTargetsToLaunch();
foreach (var result in results)
{
    Console.WriteLine(result.Target.Name + " => " + result.Status);
    if (result.Errors != null)
        foreach (var resultError in result.Errors)
        {
            Console.WriteLine(resultError.Details);
        }
}

JsonStorageProvider<AutostartConfiguration> storageProvider = new();
storageProvider.Persist("autoStartConfig", config);
config = storageProvider.Load("autoStartConfig");
Console.WriteLine(config);