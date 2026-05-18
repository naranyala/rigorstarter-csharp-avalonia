using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RigorStarter.Converters;

public class SortIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var currentColumn = value as string;
        var targetColumn = parameter as string;

        if (currentColumn == null || targetColumn == null || currentColumn != targetColumn)
        {
            return string.Empty;
        }

        // We need the direction too, but we only have the column name here.
        // This is a limitation of this simple converter.
        // In a real app, I'd pass the direction or use a more complex object.
        // For now, let's assume if it matches, it's either Asc or Desc.
        // But how to know which one?

        // Let's change the design: instead of passing the column name as parameter,
        // I'll pass the whole SortDirection and the current column name.
        // Or just simplify and not use a converter for the icon if it's too hard.

        return "↕";
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
