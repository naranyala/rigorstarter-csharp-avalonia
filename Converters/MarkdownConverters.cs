using System;
using System.Globalization;
using Avalonia.Data.Converters;
using RigorStarter.ViewModels;

namespace RigorStarter.Converters;

public class ModeToButtonTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MarkdownMode mode)
        {
            return mode == MarkdownMode.Edit ? "View Mode" : "Edit Mode";
        }
        return "Toggle Mode";
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

public class ModeToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MarkdownMode mode && parameter is string targetMode)
        {
            return mode.ToString() == targetMode;
        }
        return false;
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
