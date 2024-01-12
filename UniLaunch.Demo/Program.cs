using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;

var config = new AutostartConfiguration()
{
    Targets =
    {
        new ApplicationTarget
        {
            Name = "iTerm",
            Executable = "/Applications/iTerm.app",
        },
        new ApplicationTarget
        {
            Name = "Slack",
            Executable = "/Applications/Slack.app",
        }
    },
    RuleSets =
    {
        new RuleSet
        {
            Name = "Always",
            Rules = new List<IRule>
            {
                new AlwaysRule
                {
                    Name = "Always"
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

var engine = new AutoStartEngine(config);
engine.WaitForAllTargetsToLaunch();

BinaryStorageProvider<AutostartConfiguration> storageProvider = new();
storageProvider.Persist("autoStartConfig", config);
storageProvider.Load("autoStartConfig");
