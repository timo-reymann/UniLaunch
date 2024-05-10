using System;
using System.Globalization;
using Avalonia.Data.Converters;
using UniLaunch.UI.Assets;

namespace UniLaunch.UI.Converter;

public class LocaleDisplayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string)
        {
            return null;
        }

        return Resources.ResourceManager.GetString("LocaleName" + value.ToString()!.ToUpper(),Resources.Culture);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}