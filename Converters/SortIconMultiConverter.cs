using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using RigorStarter.ViewModels;

namespace RigorStarter.Converters;

public class SortIconMultiConverter : IMultiValueConverter
{
    public object? Convert(
        IList<object?> values,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (
            values.Count < 3
            || !(values[0] is string currentColumn)
            || !(values[1] is SortDirection direction)
            || !(parameter is string targetColumn)
        )
        {
            return string.Empty;
        }

        if (currentColumn != targetColumn)
        {
            return string.Empty;
        }

        return direction switch
        {
            SortDirection.Ascending => " ▲",
            SortDirection.Descending => " ▼",
            _ => string.Empty,
        };
    }

    public object? ConvertBack(
        IList<object?> values,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
