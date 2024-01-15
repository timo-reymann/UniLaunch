namespace UniLaunch.Core.Rules;

[Serializable]
public class RuleSet
{
    public string Name { get; init; }

    public List<Rule> Rules { get; init; }

    public bool MatchAll(ExecutionContext context) => Rules.All(r => r.Match(context));

    public bool MatchAll(ExecutionContext context, ISet<Type> enabledRuleTypes) =>
        Rules.Where(r => enabledRuleTypes.Contains(r.GetType()))
            .All(r => r.Match(context));
}