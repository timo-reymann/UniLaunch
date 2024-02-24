using UniLaunch.Core.Storage;
using UniLaunch.Core.Storage.Serialization;

namespace UniLaunch.Core.Rules;

/// <summary>
/// Match based on a given time frame
/// </summary>
[PropertyValueForSerialization("time")]
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