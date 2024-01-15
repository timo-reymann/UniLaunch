using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Tests.Rules;

public static class ContextFixtures
{
    public static ExecutionContext Now()
    {
        return new ExecutionContext(DateTime.Now);
    }

    public static ExecutionContext At(DateTime time)
    {
        return new ExecutionContext(time);
    }

    public static ExecutionContext At(DayOfWeek dayOfWeek)
    {
        var baseDate = new DateTime(1999, 1, 1);
        return At(baseDate.AddDays((int)dayOfWeek - (int)baseDate.DayOfWeek));
    }
}