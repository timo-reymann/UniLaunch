using System;
using Avalonia.Data.Converters;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;

namespace UniLaunch.UI.Converter;

public static class TypeConverter
{
    public static readonly IValueConverter TargetType = new FuncValueConverter<Type, string>(ConvertTargetType);
    public static readonly IValueConverter RuleType = new FuncValueConverter<Type, string>(ConvertRuleType);

    // Workaround converter to fix the freaking broken ListBox scroll and gridsplitter shit
    public static readonly IValueConverter ListBoxHeightConverter =
        new FuncValueConverter<double, double>(height => height - 30.0);

    private static string ConvertTargetType(Type? t)
    {
        if (t == null)
        {
            return string.Empty;
        }

        var instance = Activator.CreateInstance(t) as Target;
        return instance?.TargetType ?? string.Empty;
    }

    private static string ConvertRuleType(Type? t)
    {
        if (t == null)
        {
            return string.Empty;
        }

        var instance = Activator.CreateInstance(t) as Rule;
        return instance?.RuleName ?? string.Empty;
    }
}