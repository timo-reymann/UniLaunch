using Newtonsoft.Json;

namespace UniLaunch.Core.Rules;

[Serializable]
public abstract class Rule
{
    /// <summary>
    /// Match checks if the rule matches given the execution context.
    /// </summary>
    /// <param name="context">Context given to rule</param>
    /// <returns>True in case if the rule matches or False if not</returns>
    public abstract bool Match(ExecutionContext context);
    
    public abstract string RuleName { get; }
}