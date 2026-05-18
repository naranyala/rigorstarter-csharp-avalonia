using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using Xunit;

namespace RigorStarter.Tests;

public class ThemeServiceTests
{
    [Fact]
    public void DefaultTheme_ShouldBeLight()
    {
        var service = new ThemeService();
        Assert.False(service.IsDarkTheme);
    }

    [Fact]
    public void ToggleTheme_ShouldChangeState()
    {
        var service = new ThemeService();
        bool initial = service.IsDarkTheme;

        service.IsDarkTheme = !service.IsDarkTheme;
        Assert.NotEqual(initial, service.IsDarkTheme);
    }

    [Fact]
    public void ApplyTheme_WhenApplicationIsNull_ShouldNotThrow()
    {
        var service = new ThemeService();
        // Application.Current is null in test environment
        var exception = Record.Exception(() => service.ApplyTheme());
        Assert.Null(exception);
    }

    [Fact]
    public void IsDarkTheme_ShouldBeSettable()
    {
        var service = new ThemeService();
        service.IsDarkTheme = true;
        Assert.True(service.IsDarkTheme);

        service.IsDarkTheme = false;
        Assert.False(service.IsDarkTheme);
    }
}
