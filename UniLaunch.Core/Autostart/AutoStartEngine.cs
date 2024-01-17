using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Storage.Serialization;
using UniLaunch.Core.Targets;
using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Autostart;

public class AutoStartEngine
{
    private AutostartConfiguration? Configuration { get; set; }
    public StorageProvider<AutostartConfiguration> DefaultStorageProvider { get; private set; }
    private List<StorageProvider<AutostartConfiguration>> AvailableStoreProviders { get; set; } = new();

    private readonly HashSet<Type> _enabledTargetTypes = new();
    private readonly HashSet<Type> _enabledRuleTypes = new();

    private ExecutionContext CreateContext() => new(DateTime.Now);

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
        var targetType = typeof(T);
        _enabledTargetTypes.Add(targetType);
        CustomTypeRegistry.Register(targetType);
        return this;
    }

    public AutoStartEngine RegisterRule<T>() where T : Rule
    {
        var ruleType = typeof(T);
        _enabledRuleTypes.Add(ruleType);
        CustomTypeRegistry.Register(ruleType);
        return this;
    }

    public AutoStartEngine ApplyConfiguration(AutostartConfiguration config)
    {
        Configuration = config;
        return this;
    }
    
    public AutoStartEngine RegisterStorageProvider<T>() where T : StorageProvider<AutostartConfiguration> => 
        RegisterStorageProvider<T>(false);

    public AutoStartEngine RegisterStorageProvider<T>(bool setDefault) where T : StorageProvider<AutostartConfiguration>
    {
        var provider = (T)Activator.CreateInstance(typeof(T))!;
        
        AvailableStoreProviders.Add(provider);
        if (setDefault)
        {
            DefaultStorageProvider = provider;
        }

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