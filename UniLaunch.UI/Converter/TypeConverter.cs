using System;
using Avalonia.Data.Converters;
using UniLaunch.Core.Targets;

namespace UniLaunch.UI.Converter;

public static class TypeConverter
{
    public static readonly IValueConverter TargetType = new FuncValueConverter<Type, string>(ConvertTargetType);

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