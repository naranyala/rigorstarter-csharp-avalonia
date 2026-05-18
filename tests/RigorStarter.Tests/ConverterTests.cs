using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using RigorStarter.Converters;
using RigorStarter.Shared.Utilities;
using Xunit;

namespace RigorStarter.Tests;

public class ConverterTests
{
    [Theory]
    [InlineData(true, nameof(Brushes.LightGreen))]
    [InlineData(false, nameof(Brushes.LightPink))]
    public void SuccessToColorConverter_ShouldMapBoolToColor(object input, string expectedBrushName)
    {
        var converter = new SuccessToColorConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;

        var expected =
            typeof(Brushes).GetProperty(expectedBrushName)?.GetValue(null) as ISolidColorBrush;
        Assert.NotNull(expected);
        Assert.NotNull(result);
        Assert.Equal(expected.Color, result.Color);
    }

    [Fact]
    public void SuccessToColorConverter_NonBool_ShouldReturnWhite()
    {
        var converter = new SuccessToColorConverter();
        var result =
            converter.Convert(null, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;
        Assert.NotNull(result);
        Assert.Equal(Colors.White, result.Color);
    }

    [Theory]
    [InlineData(BadgeStatus.Info, nameof(Brushes.LightBlue))]
    [InlineData(BadgeStatus.Success, nameof(Brushes.LightGreen))]
    [InlineData(BadgeStatus.Warning, nameof(Brushes.LightYellow))]
    [InlineData(BadgeStatus.Error, nameof(Brushes.LightCoral))]
    public void BadgeStatusToColorConverter_ShouldMapKnownStatuses(
        BadgeStatus status,
        string expectedBrushName
    )
    {
        var converter = new BadgeStatusToColorConverter();
        var result =
            converter.Convert(status, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;

        var expected =
            typeof(Brushes).GetProperty(expectedBrushName)?.GetValue(null) as ISolidColorBrush;
        Assert.NotNull(expected);
        Assert.NotNull(result);
        Assert.Equal(expected.Color, result.Color);
    }

    [Theory]
    [InlineData((BadgeStatus)99)]
    [InlineData(null)]
    public void BadgeStatusToColorConverter_UnknownOrNull_ShouldReturnLightGray(object? input)
    {
        var converter = new BadgeStatusToColorConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;
        Assert.NotNull(result);
        Assert.Equal(Colors.LightGray, result.Color);
    }

    [Theory]
    [InlineData(true, nameof(Brushes.Green))]
    [InlineData(false, nameof(Brushes.Red))]
    public void SuccessToBorderConverter_ShouldMapBoolToBorder(
        object input,
        string expectedBrushName
    )
    {
        var converter = new SuccessToBorderConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;

        var expected =
            typeof(Brushes).GetProperty(expectedBrushName)?.GetValue(null) as ISolidColorBrush;
        Assert.NotNull(expected);
        Assert.NotNull(result);
        Assert.Equal(expected.Color, result.Color);
    }

    [Fact]
    public void SuccessToBorderConverter_NonBool_ShouldReturnGray()
    {
        var converter = new SuccessToBorderConverter();
        var result =
            converter.Convert(null, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Theory]
    [InlineData(true, nameof(Brushes.DarkGreen))]
    [InlineData(false, nameof(Brushes.DarkRed))]
    public void SuccessToTextConverter_ShouldMapBoolToTextColor(
        object input,
        string expectedBrushName
    )
    {
        var converter = new SuccessToTextConverter();
        var result =
            converter.Convert(input, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;

        var expected =
            typeof(Brushes).GetProperty(expectedBrushName)?.GetValue(null) as ISolidColorBrush;
        Assert.NotNull(expected);
        Assert.NotNull(result);
        Assert.Equal(expected.Color, result.Color);
    }

    [Fact]
    public void SuccessToTextConverter_NonBool_ShouldReturnBlack()
    {
        var converter = new SuccessToTextConverter();
        var result =
            converter.Convert(null, typeof(IBrush), null, CultureInfo.InvariantCulture)
            as ISolidColorBrush;
        Assert.NotNull(result);
        Assert.Equal(Colors.Black, result.Color);
    }

    [Theory]
    [InlineData(true, "\u2600\ufe0f Light")]
    [InlineData(false, "\U0001f319 Dark")]
    public void ThemeIconConverter_ShouldMapBoolToString(object input, string expected)
    {
        var converter = new ThemeIconConverter();
        var result = converter.Convert(input, typeof(string), null, CultureInfo.InvariantCulture);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ThemeIconConverter_Null_ShouldReturnDarkIcon()
    {
        var converter = new ThemeIconConverter();
        var result = converter.Convert(null, typeof(string), null, CultureInfo.InvariantCulture);
        Assert.Equal("\U0001f319 Dark", result);
    }

    [Fact]
    public void SuccessToColorConverter_ConvertBack_ShouldThrow()
    {
        IValueConverter converter = new SuccessToColorConverter();
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(null, typeof(bool), null, CultureInfo.InvariantCulture)
        );
    }

    [Fact]
    public void BadgeStatusToColorConverter_ConvertBack_ShouldThrow()
    {
        IValueConverter converter = new BadgeStatusToColorConverter();
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(null, typeof(bool), null, CultureInfo.InvariantCulture)
        );
    }

    [Fact]
    public void SuccessToBorderConverter_ConvertBack_ShouldThrow()
    {
        IValueConverter converter = new SuccessToBorderConverter();
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(null, typeof(bool), null, CultureInfo.InvariantCulture)
        );
    }

    [Fact]
    public void SuccessToTextConverter_ConvertBack_ShouldThrow()
    {
        IValueConverter converter = new SuccessToTextConverter();
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(null, typeof(bool), null, CultureInfo.InvariantCulture)
        );
    }

    [Fact]
    public void ThemeIconConverter_ConvertBack_ShouldThrow()
    {
        IValueConverter converter = new ThemeIconConverter();
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(null, typeof(bool), null, CultureInfo.InvariantCulture)
        );
    }
}
