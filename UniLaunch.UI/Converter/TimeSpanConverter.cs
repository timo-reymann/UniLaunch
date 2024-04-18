using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace UniLaunch.UI.Converter;

public class TimeSpanConverter : IValueConverter
{
    private bool isStringTargetType(Type targetType) => targetType == typeof(string);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var timeSpan = (value as TimeSpan?);
        if (isStringTargetType(targetType))
        {
            return timeSpan != null ? timeSpan?.TotalSeconds.ToString() : "";
        }

        return timeSpan != null ? timeSpan?.TotalSeconds : 0.0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        
        if (isStringTargetType(value.GetType()))
        {
            var str = value as string;
            if (str.Trim() == "")
            {
                return null;
            }
            return TimeSpan.FromSeconds(Double.Parse(str));
        }

        var num = value as Double?;
        return TimeSpan.FromSeconds(num!.Value);
    }
}