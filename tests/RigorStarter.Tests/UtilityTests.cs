using System;
using RigorStarter.Utilities;
using Xunit;

namespace RigorStarter.Tests;

public class UtilityTests
{
    [Fact]
    public void NetworkUtility_ShouldReturnNonEmptySummary()
    {
        var result = NetworkUtility.GetNetworkSummary();
        var summary = result is UtilityResult ur ? ur.Message : result?.ToString();
        Assert.False(string.IsNullOrWhiteSpace(summary));
    }

    [Fact]
    public void DiskUtility_ShouldReturnNonEmptySummary()
    {
        var result = DiskUtility.GetDiskSummary();
        var summary = result is UtilityResult ur ? ur.Message : result?.ToString();
        Assert.False(string.IsNullOrWhiteSpace(summary));
    }

    [Fact]
    public void SystemInfoUtility_ShouldReturnNonEmptySummary()
    {
        var result = SystemInfoUtility.GetSystemSummary();
        var summary = result is UtilityResult ur ? ur.Message : result?.ToString();
        Assert.False(string.IsNullOrWhiteSpace(summary));
    }

    [Fact]
    public void MemoryUtility_ShouldReturnValidResult()
    {
        var result = MemoryUtility.GetMemorySummary();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public void ProcessUtility_ShouldReturnTopProcesses()
    {
        var result = ProcessUtility.GetTopProcesses();
        var summary = result is UtilityResult ur ? ur.Message : result?.ToString();
        Assert.False(string.IsNullOrWhiteSpace(summary));
        Assert.Contains("Memory", summary);
    }

    [Fact]
    public void XdgPaths_ShouldReturnValidPaths()
    {
        var appName = "TestApp";
        Assert.False(string.IsNullOrWhiteSpace(XdgPaths.GetConfigDir(appName)));
        Assert.False(string.IsNullOrWhiteSpace(XdgPaths.GetDataDir(appName)));
        Assert.False(string.IsNullOrWhiteSpace(XdgPaths.GetCacheDir(appName)));
    }

    [Fact]
    public void ProcFS_ShouldReadMemInfo()
    {
        var mem = ProcFS.ReadProcFile("meminfo");
        Assert.NotEmpty(mem);
        Assert.True(mem.ContainsKey("MemTotal") || mem.ContainsKey("Error"));
    }

    [Fact]
    public void OsRelease_ShouldReturnDistroInfo()
    {
        var info = OsRelease.GetDistroInfo();
        Assert.NotNull(info);
        Assert.False(string.IsNullOrWhiteSpace(info.PrettyName));
    }

    [Fact]
    public void ConfigManager_ShouldSaveAndLoadConfig()
    {
        var appName = "TestConfigApp";
        var fileName = "settings.yaml";
        var config = new TestConfig { Theme = "Dark", FontSize = 14 };

        ConfigManager.SaveConfig(appName, fileName, config);
        var loaded = ConfigManager.LoadConfig<TestConfig>(appName, fileName);

        Assert.Equal(config.Theme, loaded.Theme);
        Assert.Equal(config.FontSize, loaded.FontSize);
    }

    private class TestConfig
    {
        public string Theme { get; set; } = "Light";
        public int FontSize { get; set; } = 12;
    }
}
