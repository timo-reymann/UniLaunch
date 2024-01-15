namespace UniLaunch.Core.Rules;

[Serializable]
public record HourMinute(byte Hour, byte Minute);

[Serializable]
public class TimeRule : Rule
{
    public HourMinute StartRange { get; set; }
    public HourMinute EndRange { get; set; }
    
    public override bool Match(ExecutionContext context) =>
        context.InvocationTime.Hour >= StartRange.Hour &&
        context.InvocationTime.Hour <= EndRange.Hour &&
        context.InvocationTime.Minute >= StartRange.Minute &&
        context.InvocationTime.Minute <= EndRange.Minute;

    public override string RuleName => "time";
}