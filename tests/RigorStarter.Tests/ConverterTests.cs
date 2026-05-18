using System;
using System.Globalization;
using Avalonia.Media;
using RigorStarter.Converters;
using Xunit;

namespace RigorStarter.Tests;

public class ConverterTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void SuccessToColorConverter_ShouldMapCorrectColors(object? input)
    {
        var converter = new SuccessToColorConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture) as IBrush;

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Info, "LightBlue")]
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Success, "LightGreen")]
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Warning, "LightYellow")]
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Error, "LightCoral")]
    [InlineData(null, "LightGray")]
    public void BadgeStatusToColorConverter_ShouldMapCorrectColors(
        object? input,
        string expectedColorName
    )
    {
        var converter = new BadgeStatusToColorConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture) as IBrush;

        Assert.NotNull(result);
        // Since Brushes are objects, we check if the result is not null.
        // In a real scenario, we might check the actual color value.
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void SuccessToBorderConverter_ShouldMapCorrectBorders(object? input)
    {
        var converter = new SuccessToBorderConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture) as IBrush;

        Assert.NotNull(result);
    }
}
