namespace UniLaunch.Core.Rules;

[Serializable]
public class RuleSet
{
    public string Name { get; init; }
    
    public List<Rule> Rules { get; init; }

    public bool MatchAll(ExecutionContext context) => Rules.All(r => r.Match(context));
}