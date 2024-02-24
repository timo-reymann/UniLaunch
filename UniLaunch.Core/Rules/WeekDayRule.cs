using UniLaunch.Core.Storage.Serialization;

namespace UniLaunch.Core.Rules;

/// <summary>
/// Match based on given weekdays
/// </summary>
[PropertyValueForSerialization("week-day")]
public class WeekDayRule : Rule
{
    public List<DayOfWeek> DaysOfWeekToRun { get; set; } = new();

    public override bool Match(ExecutionContext context) => DaysOfWeekToRun.Contains(context.InvocationTime.DayOfWeek);
    public override string RuleName => "week-day";
}