namespace UniLaunch.Core.Rules;

[Serializable]
public class AlwaysRule : Rule
{
    public override bool Match(ExecutionContext context) => true;
}