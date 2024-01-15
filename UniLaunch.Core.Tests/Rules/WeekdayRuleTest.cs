using UniLaunch.Core.Rules;
using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Tests.Rules;

using System;
using Xunit;

public class WeekDayRuleTests
{
    [Theory]
    [InlineData(DayOfWeek.Monday, true)]
    [InlineData(DayOfWeek.Wednesday, true)]
    [InlineData(DayOfWeek.Friday, true)]
    [InlineData(DayOfWeek.Tuesday, false)]
    [InlineData(DayOfWeek.Thursday, false)]
    public void Match_ReturnsCorrectResult(DayOfWeek invocationDay, bool expectedResult)
    {
        // Arrange
        var rule = new WeekDayRule
        {
            DaysToRun = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
        };

        var context = ContextFixtures.At(invocationDay);

        // Act & Assert
        Assert.Equal(expectedResult, rule.Match(context));
    }
}