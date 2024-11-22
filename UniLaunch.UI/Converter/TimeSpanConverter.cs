using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace UniLaunch.UI.Converter;

public class TimeSpanConverter : IValueConverter
{
    private bool IsStringTargetType(Type targetType) => targetType == typeof(string);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var timeSpan = (value as TimeSpan?);
        if (IsStringTargetType(targetType))
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
        
        if (IsStringTargetType(value.GetType()))
        {
            if (value == null)
            {
                return null;
            }
            
            var str = value as string;
            if (str!.Trim() == "")
            {
                return null;
            }
            return TimeSpan.FromSeconds(double.Parse(str));
        }

        var num = value as Double?;
        return TimeSpan.FromSeconds(num!.Value);
    }
}