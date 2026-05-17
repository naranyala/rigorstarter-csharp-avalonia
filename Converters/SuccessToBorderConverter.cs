using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace RigorStarter.Converters;

public class SuccessToBorderConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isSuccess)
        {
            return isSuccess ? Brushes.Green : Brushes.Red;
        }
        return Brushes.Gray;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
