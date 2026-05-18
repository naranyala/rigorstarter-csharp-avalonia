using System;
using System.Threading.Tasks;
using RigorStarter.Shared.Utilities;
using Xunit;

namespace RigorStarter.Tests;

public class SystemHubTests
{
    [Fact]
    public void SystemHub_ShouldRouteToOsRelease()
    {
        var info = SystemHub.Distro;
        Assert.NotNull(info);
        Assert.False(string.IsNullOrWhiteSpace(info.PrettyName));
    }

    [Fact]
    public void SystemHub_ShouldRouteToXdgPaths()
    {
        var path = SystemHub.ConfigPath("TestApp");
        Assert.False(string.IsNullOrWhiteSpace(path));
        Assert.Contains("TestApp", path);

        var dataPath = SystemHub.DataPath("TestApp");
        Assert.False(string.IsNullOrWhiteSpace(dataPath));

        var cachePath = SystemHub.CachePath("TestApp");
        Assert.False(string.IsNullOrWhiteSpace(cachePath));
    }

    [Fact]
    public void SystemHub_ShouldRouteToProcFS()
    {
        var ram = SystemHub.TotalRam;
        Assert.True(ram > 0);

        var memInfo = SystemHub.MemInfo;
        Assert.NotEmpty(memInfo);
    }

    [Fact]
    public async Task SystemHub_ShouldRouteToPackageManager()
    {
        var pkg = await SystemHub.GetPkgManager();
        Assert.True(Enum.IsDefined(typeof(PackageManagerType), pkg));
    }

    [Fact]
    public void SystemHub_ShouldProvideLogging()
    {
        var exception = Record.Exception(() => SystemHub.LogInfo("Test info message"));
        Assert.Null(exception);

        exception = Record.Exception(() => SystemHub.LogError("Test error message"));
        Assert.Null(exception);
    }

    [Fact]
    public async Task SystemHub_Run_ShouldExecuteCommand()
    {
        var result = await SystemHub.Run("echo", "hello world");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task SystemHub_Notify_ShouldNotThrow()
    {
        var exception = await Record.ExceptionAsync(() =>
            SystemHub.Notify("Test Title", "Test Message")
        );
        Assert.Null(exception);
    }

    [Fact]
    public async Task SystemHub_GetSystemProperty_ShouldNotThrow()
    {
        var exception = await Record.ExceptionAsync(() =>
            SystemHub.GetSystemProperty("org.freedesktop.DBus", "/", "org.freedesktop.DBus", "Id")
        );
        // DBus may not be available in test environment
        Assert.Null(exception);
    }

    [Fact]
    public async Task SystemHub_IsServiceRunning_ShouldNotThrow()
    {
        var exception = await Record.ExceptionAsync(() =>
            SystemHub.IsServiceRunning("nonexistent.service")
        );
        Assert.Null(exception);
    }

    [Fact]
    public void SystemHub_LoadConfig_ShouldReturnDefault()
    {
        var config = SystemHub.LoadConfig<TestConfig>("nonexistent.yaml");
        Assert.NotNull(config);
    }

    [Fact]
    public void SystemHub_SaveConfig_ShouldNotThrow()
    {
        var config = new TestConfig { Value = "test" };
        var exception = Record.Exception(() => SystemHub.SaveConfig("test.yaml", config));
        Assert.Null(exception);
    }

    private class TestConfig
    {
        public string Value { get; set; } = "default";
    }
}
