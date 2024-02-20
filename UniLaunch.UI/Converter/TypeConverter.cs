using System;
using Avalonia.Data.Converters;
using UniLaunch.Core.Targets;

namespace UniLaunch.UI.Converter;

public static class TypeConverter
{
    public static readonly IValueConverter TargetType = new FuncValueConverter<Type, string>(ConvertTargetType);

    // Workaround converter to fix the freaking broken ListBox scroll and gridsplitter shit
    public static readonly IValueConverter ListBoxHeightConverter =
        new FuncValueConverter<double, double>( height => height - 30.0);
    
    private static string ConvertTargetType(Type? t)
    {
        if (t == null)
        {
            return string.Empty;
        }

        var instance = Activator.CreateInstance(t) as Target;
        return instance?.TargetType ?? string.Empty;
    }
}