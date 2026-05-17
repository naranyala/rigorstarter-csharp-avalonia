using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RigorStarter.Converters;

public class ThemeIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isDarkTheme)
        {
            return isDarkTheme ? "☀️ Light" : "🌙 Dark";
        }
        return "🌙 Dark";
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
