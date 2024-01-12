using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Tests.Rules;

public static class ContextFixtures
{
    public static ExecutionContext Now()
    {
        return new ExecutionContext(DateTime.Now);
    }
}