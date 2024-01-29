using Newtonsoft.Json;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Storage.Serialization;

namespace UniLaunch.Core.Rules;

/// <summary>
/// Implement a rule, which evaluates if a given target should be executed
/// </summary>
[Serializable]
[PropertyBasedSerialization("ruleName")]
public abstract class Rule 
{
    /// <summary>
    /// Match checks if the rule matches given the execution context.
    /// </summary>
    /// <param name="context">Context given to rule</param>
    /// <returns>True in case if the rule matches or False if not</returns>
    public abstract bool Match(ExecutionContext context);

    /// <summary>
    /// RuleName for configuration serialization and deserialization.
    ///
    /// It needs to be unique for each rule
    /// </summary>
    public abstract string RuleName { get; }
}