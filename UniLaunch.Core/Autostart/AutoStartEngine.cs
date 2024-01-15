using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;
using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Autostart;

public class AutoStartEngine
{
    private AutostartConfiguration? Configuration { get; set; }

    private HashSet<Type> _enabledTargetTypes = new();
    private HashSet<Type> _enabledRuleTypes = new();

    private ExecutionContext CreateContext() => new ExecutionContext(DateTime.Now);

    private IEnumerable<Target> GetTargets()
    {
        if (Configuration == null)
        {
            yield break;
        }

        var executionContext = CreateContext();

        foreach (var entry in Configuration.Entries)
        {
            var ruleSet = Configuration.GetRuleSetByName(entry.RuleSetName)!;
            if (!ruleSet.MatchAll(executionContext, _enabledRuleTypes))
            {
                continue;
            }

            var target = Configuration.GetTargetByName(entry.TargetName)!;
            if (_enabledTargetTypes.Contains(target.GetType()))
            {
                yield return target;
            }
        }
    }

    public AutoStartEngine RegisterTarget<T>() where T : Target
    {
        _enabledTargetTypes.Add(typeof(T));
        return this;
    }

    public AutoStartEngine RegisterRule<T>() where T : Rule
    {
        _enabledRuleTypes.Add(typeof(T));
        return this;
    }

    public AutoStartEngine ApplyConfiguration(AutostartConfiguration config)
    {
        Configuration = config;
        return this;
    }

    private List<Task<TargetInvokeResult>> GetTargetInvokes()
    {
        return GetTargets()
            .Select(target => target.Invoke())
            .ToList();
    }

    public Task<TargetInvokeResult[]> WaitForAllTargetsToLaunch() => Task.WhenAll(GetTargetInvokes());
}