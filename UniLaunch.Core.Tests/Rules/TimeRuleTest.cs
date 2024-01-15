using UniLaunch.Core.Rules;
using ExecutionContext = UniLaunch.Core.Rules.ExecutionContext;

namespace UniLaunch.Core.Tests.Rules;
using System;
using Xunit;

public class TimeRuleTests
{
    [Theory]
    [InlineData(10, 30, 12, 45, 10, 30)] 
    [InlineData(8, 0, 9, 0, 8, 45)] 
    [InlineData(18, 0, 20, 0, 20, 0)]
    public void Match_ReturnsTrue_WhenInvocationTimeWithinRange(
        byte startHour, byte startMinute, byte endHour, byte endMinute, byte invocationHour, byte invocationMinute)
    {
        // Arrange
        var rule = new TimeRule
        {
            StartRange = new TimeOnly(startHour, startMinute),
            EndRange = new TimeOnly(endHour, endMinute)
        };

        var context = ContextFixtures.At(new DateTime(2022, 1, 1, invocationHour, invocationMinute, 0));

        // Act & Assert
        Assert.True(rule.Match(context));
    }

    [Theory]
    [InlineData(10, 30, 12, 45, 9, 30)] // Outside the time range (before start)
    [InlineData(8, 0, 9, 0, 9, 15)] // Outside the time range (after start)
    [InlineData(18, 0, 20, 0, 20, 45)] // Outside the time range (after end)
    public void Match_ReturnsFalse_WhenInvocationTimeOutsideRange(
        byte startHour, byte startMinute, byte endHour, byte endMinute, byte invocationHour, byte invocationMinute)
    {
        // Arrange
        var rule = new TimeRule
        {
            StartRange = new TimeOnly(startHour, startMinute),
            EndRange = new TimeOnly(endHour, endMinute)
        };

        var context = ContextFixtures.At(new DateTime(2022, 1, 1, invocationHour, invocationMinute, 0));

        // Act & Assert
        Assert.False(rule.Match(context));
    }
}
