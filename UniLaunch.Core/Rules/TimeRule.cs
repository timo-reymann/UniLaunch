namespace UniLaunch.Core.Rules;

[Serializable]
public class TimeRule : Rule
{
    public TimeOnly StartRange { get; set; }
    public TimeOnly EndRange { get; set; }
    
    public override bool Match(ExecutionContext context) =>
        IsWithinRange(context.InvocationTime);

    private bool IsWithinRange(DateTime executionTime) =>
        executionTime.TimeOfDay >= TimeSpan.FromHours(StartRange.Hour) + TimeSpan.FromMinutes(StartRange.Minute)
        && executionTime.TimeOfDay <= TimeSpan.FromHours(EndRange.Hour) + TimeSpan.FromMinutes(EndRange.Minute);

    public override string RuleName => "time";
}