using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Storage.Serialization;
using UniLaunch.Core.Targets;
using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Autostart;

public class UniLaunchEngine
{
    public StorageProvider<UniLaunchConfiguration> DefaultStorageProvider { get; private set; }
    public UniLaunchConfiguration? Configuration { get; private set; } = new();
    public List<StorageProvider<UniLaunchConfiguration>> AvailableStoreProviders { get; private set; } = new();

    /// <summary>
    /// Global shared instance of the engine
    /// </summary>
    public static readonly UniLaunchEngine Instance = new();

    private string? ConfigFilePath { get; set; }
    private FileLocator ConfigFileLocator { get; set; }

    private readonly HashSet<Type> _enabledTargetTypes = new();
    private readonly HashSet<Type> _enabledRuleTypes = new();

    private UniLaunchEngine()
    {
    }

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

    /// <summary>
    /// Enable a target for the engine and enable its serialization and deserialization
    /// </summary>
    /// <typeparam name="T">Target implementation</typeparam>
    /// <returns></returns>
    public UniLaunchEngine RegisterTarget<T>() where T : Target
    {
        var targetType = typeof(T);
        _enabledTargetTypes.Add(targetType);
        CustomTypeRegistry.Register(targetType);
        return this;
    }

    /// <summary>
    /// Enable a rule for use with the engine and enable serialization/deserialization
    /// </summary>
    /// <typeparam name="T">Rule implementation</typeparam>
    /// <returns></returns>
    public UniLaunchEngine RegisterRule<T>() where T : Rule
    {
        var ruleType = typeof(T);
        _enabledRuleTypes.Add(ruleType);
        CustomTypeRegistry.Register(ruleType);
        return this;
    }

    private string LocateConfigFile()
    {
        var defaultConfig = ConfigFileLocator.Locate(DefaultStorageProvider.Extension);
        if (defaultConfig != null)
        {
            return defaultConfig;
        }

        foreach (var alternativeProvider in AvailableStoreProviders)
        {
            var alternativeConfigFile = ConfigFileLocator.Locate(alternativeProvider.Extension);
            if (alternativeConfigFile == null)
            {
                continue;
            }

            DefaultStorageProvider = alternativeProvider;
            return alternativeConfigFile;
        }

        return ConfigFileLocator.LocateWithFallback(DefaultStorageProvider.Extension);
    }

    /// <summary>
    /// Locate and parse the configuration file as located by the default location provider or any other non default
    /// in case the file has been found
    /// </summary>
    /// <param name="ignoreErrors">Ignore errors due to missing file etc.</param>
    /// <returns></returns>
    public UniLaunchEngine LocateAndParseConfigFile(bool ignoreErrors = true)
    {
        ConfigFilePath = LocateConfigFile();
        if (ignoreErrors)
        {
            try
            {
                Configuration = DefaultStorageProvider.Load(ConfigFilePath);
            }
            catch (StorageException)
            {
            }
        }
        else
        {
            Configuration = DefaultStorageProvider.Load(ConfigFilePath);
        }

        return this;
    }

    /// <summary>
    /// Override the current configuration for autostart engine
    /// </summary>
    /// <param name="config">Configuration to set</param>
    /// <param name="configFilePath">Override path to config file without extension</param>
    /// <param name="persist">Persist the configuration to the configuration file used</param>
    /// <returns></returns>
    public UniLaunchEngine OverrideConfiguration(
        UniLaunchConfiguration config,
        string? configFilePath = null,
        bool persist = true)
    {
        Configuration = config;

        if (configFilePath != null)
        {
            ConfigFilePath = configFilePath;
        }

        if (persist)
        {
            PersistCurrentConfiguration();
        }
        
        return this;
    }

    /// <summary>
    /// Persist the current configuration using default storage provider to discovered config file path
    /// </summary>
    /// <returns></returns>
    public UniLaunchEngine PersistCurrentConfiguration()
    {
        if (Configuration == null)
        {
            return this;
        }

        DefaultStorageProvider.Persist(ConfigFilePath, Configuration!);
        return this;
    }

    /// <summary>
    /// Set the config file locator used to find and set up for detecting backing config file for storage
    /// </summary>
    /// <param name="locator">FileLocator</param>
    /// <returns></returns>
    public UniLaunchEngine UseConfigFileLocator(FileLocator locator)
    {
        ConfigFileLocator = locator;
        return this;
    }

    public UniLaunchEngine RegisterStorageProvider<T>() where T : StorageProvider<UniLaunchConfiguration> =>
        RegisterStorageProvider<T>(false);

    /// <summary>
    /// Register a storage provider
    /// </summary>
    /// <param name="setDefault">Set as default</param>
    /// <typeparam name="T">Storage provider implementation</typeparam>
    /// <returns></returns>
    public UniLaunchEngine RegisterStorageProvider<T>(bool setDefault) where T : StorageProvider<UniLaunchConfiguration>
    {
        var provider = (T)Activator.CreateInstance(typeof(T))!;

        AvailableStoreProviders.Add(provider);
        if (setDefault)
        {
            DefaultStorageProvider = provider;
        }

        return this;
    }

    private IEnumerable<Task<TargetInvokeResult>> GetTargetInvokes()
    {
        return GetTargets()
            .Select(target => target.Invoke())
            .ToList();
    }

    public Task<TargetInvokeResult[]> WaitForAllTargetsToLaunch() => Task.WhenAll(GetTargetInvokes());
}