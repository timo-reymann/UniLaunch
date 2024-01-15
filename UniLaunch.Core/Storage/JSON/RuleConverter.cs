using UniLaunch.Core.Rules;

namespace UniLaunch.Core.Storage.JSON;

public class RuleConverter : PropertyBasedConverter
{
    protected override string PropertyName => "ruleName";
    protected override Type TypeToSerialize { get; } = typeof(Rule);

    protected override Dictionary<string, Type> TypeMapping { get; } = new()
    {
        { "always", typeof(AlwaysRule) },
        { "time", typeof(TimeRule) },
        { "week-day", typeof(WeekDayRule) }
    };
}