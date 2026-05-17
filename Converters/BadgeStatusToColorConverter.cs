using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using RigorStarter.Utilities;

namespace RigorStarter.Converters;

public class BadgeStatusToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BadgeStatus status)
        {
            return status switch
            {
                BadgeStatus.Info => Brushes.LightBlue,
                BadgeStatus.Success => Brushes.LightGreen,
                BadgeStatus.Warning => Brushes.LightYellow,
                BadgeStatus.Error => Brushes.LightCoral,
                _ => Brushes.LightGray,
            };
        }
        return Brushes.LightGray;
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
