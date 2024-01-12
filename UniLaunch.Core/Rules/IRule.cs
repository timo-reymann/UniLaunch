using UniLaunch.Core.Storage;

namespace UniLaunch.Core.Rules;

public interface IRule
{
    public string Name { get; set; }

    /// <summary>
    /// Match checks if the rule matches given the execution context.
    /// </summary>
    /// <param name="context">Context given to rule</param>
    /// <returns>True in case if the rule matches or False if not</returns>
    public bool Match(ExecutionContext context);
}