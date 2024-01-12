using UniLaunch.Core.Targets;
using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Autostart;

public class AutoStartEngine
{
    internal AutostartConfiguration Configuration { get; private set; }
    internal ExecutionContext ExecutionContext { get; private set; }

    public AutoStartEngine(AutostartConfiguration configuration)
    {
        Configuration = configuration;
        ExecutionContext = new ExecutionContext(DateTime.Now);
    }

    public IEnumerable<ITarget> GetTargets()
    {
        foreach (var entry in Configuration.Entries)
        {
            var ruleSet = Configuration.GetRuleSetByName(entry.RuleSetName)!;
            if (ruleSet.MatchAll(ExecutionContext))
            {
                yield return Configuration.GetTargetByName(entry.TargetName)!;
            }
        }
    }

    public List<Task> GetTargetInvokes()
    {
        return GetTargets()
            .Select(target => target.Invoke())
            .ToList();
    }
}