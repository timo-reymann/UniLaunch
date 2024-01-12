// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;

var config = new AutostartConfiguration();
config.Targets.Add(new ApplicationTarget()
{
    Name = "iTerm",
    Executable = "/Applications/iTerm.app"
});
config.RuleSets.Add(new RuleSet
{
    Name = "Always",
    Rules = new List<IRule>()
    {
        new AlwaysRule()
        {
            Name = "Always"
        }
    }
});
config.Entries.Add(new AutoStartEntry("Always", "iTerm"));
var engine = new AutoStartEngine(config);
engine.Evaluate();
Console.WriteLine("Startup completed");

BinaryStorageProvider<AutostartConfiguration> storageProvider = new();
storageProvider.Persist("autoStartConfig", config);
config = storageProvider.Load("autoStartConfig");
Console.WriteLine(config);