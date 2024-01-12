namespace UniLaunch.Core.Rules;

[Serializable]
public class AlwaysRule : IRule
{
    public string Name { get; set; } = "AlwaysRule";

    public bool Match(ExecutionContext context)
    {
        return true;
    }
}