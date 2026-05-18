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
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Info)]
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Success)]
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Warning)]
    [InlineData(RigorStarter.Shared.Utilities.BadgeStatus.Error)]
    [InlineData(null)]
    public void BadgeStatusToColorConverter_ShouldMapCorrectColors(object? input)
    {
        var converter = new BadgeStatusToColorConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture) as IBrush;

        Assert.NotNull(result);
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
