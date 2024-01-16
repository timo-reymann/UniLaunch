using UniLaunch.Core.Storage;

namespace UniLaunch.Core.Rules;

[PropertyValueForSerialization("week-day")]
public class WeekDayRule : Rule
{
    public DayOfWeek[] DaysOfWeekToRun { get; set; }

    public override bool Match(ExecutionContext context) => DaysOfWeekToRun.Contains(context.InvocationTime.DayOfWeek);
    public override string RuleName => "week-day";
}