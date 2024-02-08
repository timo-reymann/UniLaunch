using UniLaunch.Core.Spec;

namespace UniLaunch.Core.Rules;

/// <summary>
/// Set of multiple rules to execute in order to launch targets
/// </summary>
[Serializable]
public class RuleSet : INameable
{
    /// <summary>
    /// Name of the rulset
    /// </summary>
    public string Name { get; set; }

    // Rules contained in the set
    public List<Rule> Rules { get; set; } = new();

    /// <summary>
    /// Match all rules that match for the given execution context
    /// </summary>
    /// <param name="context">Context to evaluate</param>
    /// <returns>Matches all rules (true) or not (false)</returns>
    public bool MatchAll(ExecutionContext context) => Rules.All(r => r.Match(context));

    /// <summary>
    /// Match all rules that match for the given execution context
    /// </summary>
    /// <param name="context">Context to evaluate</param>
    /// <param name="enabledRuleTypes">Restrict rules to evaluate on the given types</param>
    /// <returns>Matches all rules (true) or not (false)</returns>
    public bool MatchAll(ExecutionContext context, ISet<Type> enabledRuleTypes) =>
        Rules.Where(r => enabledRuleTypes.Contains(r.GetType()))
            .All(r => r.Match(context));
}