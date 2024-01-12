using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Autostart;

public class AutoStartEngine
{
    public AutostartConfiguration Configuration { get; private set; }

    public AutoStartEngine(AutostartConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void Evaluate()
    {
        foreach (var entry in Configuration.Entries)
        {
            var executionContext = new ExecutionContext(DateTime.Now);
            var target = Configuration.Targets.First(t => t.Name == entry.TargetName);
            var ruleSet = Configuration.RuleSets.First(rs => rs.Name == entry.RuleSetName);
            var execute = ruleSet.Rules.All(rule => rule.Match(executionContext));
            if (execute)
            {
                target.Invoke();
            }
        }
    }
}