using UniLaunch.Core.Rules;

namespace UniLaunch.Core.Tests.Rules;

public class AlwaysRuleTests
{
    [Fact]
    public void TestAlwaysRule()
    {
        var rule = new AlwaysRule();
        Assert.True(rule.Match(ContextFixtures.Now()));
    }
}