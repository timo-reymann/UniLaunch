namespace UniLaunch.Core.Rules;

[Serializable]
public class RuleSet
{
    public string Name { get; init; }
    public List<IRule> Rules { get; init; }
}