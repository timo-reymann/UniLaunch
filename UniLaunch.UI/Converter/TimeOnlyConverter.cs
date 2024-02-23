using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace UniLaunch.UI.Converter;

public class TimeOnlyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        var timeOnly = value as TimeOnly?;
        return new TimeSpan(timeOnly!.Value.Hour, timeOnly.Value.Minute, 0);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        var timeSpan = value as TimeSpan?;
        return new TimeOnly(timeSpan!.Value.Hours, timeSpan.Value.Minutes);
    }
}