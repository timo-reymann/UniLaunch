namespace UniLaunch.Core.Rules;

[Serializable]
public class WeekDayRule : Rule
{
    public DayOfWeek[] DaysToRun { get; set; }

    public override bool Match(ExecutionContext context) => DaysToRun.Contains(context.InvocationTime.DayOfWeek);
}