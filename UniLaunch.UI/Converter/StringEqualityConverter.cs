using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace UniLaunch.UI.Converter;

public class StringEqualityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value?.Equals(parameter);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value != null && value.Equals(true) ? parameter : BindingOperations.DoNothing;
}