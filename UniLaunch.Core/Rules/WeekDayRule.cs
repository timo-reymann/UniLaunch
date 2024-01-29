using UniLaunch.Core.Storage.Serialization;

namespace UniLaunch.Core.Rules;

/// <summary>
/// Match based on given weekdays
/// </summary>
[PropertyValueForSerialization("week-day")]
public class WeekDayRule : Rule
{
    public DayOfWeek[] DaysOfWeekToRun { get; set; } = Array.Empty<DayOfWeek>();

    public override bool Match(ExecutionContext context) => DaysOfWeekToRun.Contains(context.InvocationTime.DayOfWeek);
    public override string RuleName => "week-day";
}