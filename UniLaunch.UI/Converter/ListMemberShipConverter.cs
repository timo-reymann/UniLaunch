using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace UniLaunch.UI.Converter;

public class ListMemberShipConverter : IMultiValueConverter
{
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
    
    static bool ContainsItem(object list, object item)
    {
        IEnumerable enumerable = (IEnumerable)list;
        return enumerable.Cast<object>().Any(element => object.Equals(element, item));
    }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return ContainsItem(values[0],values[1]);
    }
}