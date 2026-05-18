using System;
using System.IO;
using RigorStarter.Shared.Models;
using RigorStarter.Shared.Utilities;
using Xunit;

namespace RigorStarter.Tests;

public class UtilityTests
{
    [Fact]
    public void NetworkUtility_ShouldReturnNonEmptySummary()
    {
        var result = NetworkUtility.GetNetworkSummary();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public void DiskUtility_ShouldReturnNonEmptySummary()
    {
        var result = DiskUtility.GetDiskSummary();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public void SystemInfoUtility_ShouldReturnNonEmptySummary()
    {
        var result = SystemInfoUtility.GetSystemSummary();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public void MemoryUtility_ShouldReturnValidResult()
    {
        var result = MemoryUtility.GetMemorySummary();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Contains("Memory", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CpuUtility_ShouldReturnValidResult()
    {
        var result = CpuUtility.GetCpuSummary();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Contains("CPU", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CpuUtility_ShouldReturnPercentage()
    {
        var result = CpuUtility.GetCpuSummary();
        Assert.NotNull(result);
        Assert.Contains("%", result.Message);
    }

    [Fact]
    public void ProcessUtility_ShouldReturnTopProcesses()
    {
        var result = ProcessUtility.GetTopProcesses();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
        Assert.Contains("Memory", result.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Top 10", result.Message);
    }

    [Fact]
    public void XdgPaths_ShouldReturnValidPaths()
    {
        var appName = "TestApp";
        var configDir = XdgPaths.GetConfigDir(appName);
        var dataDir = XdgPaths.GetDataDir(appName);
        var cacheDir = XdgPaths.GetCacheDir(appName);

        Assert.False(string.IsNullOrWhiteSpace(configDir));
        Assert.False(string.IsNullOrWhiteSpace(dataDir));
        Assert.False(string.IsNullOrWhiteSpace(cacheDir));

        // All directories should contain the app name
        Assert.Contains(appName, configDir);
        Assert.Contains(appName, dataDir);
        Assert.Contains(appName, cacheDir);
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
        var appName = "TestConfigApp_" + Guid.NewGuid().ToString("N");
        var fileName = "settings.yaml";
        var config = new TestConfig { Theme = "Dark", FontSize = 14 };

        try
        {
            ConfigManager.SaveConfig(appName, fileName, config);
            var loaded = ConfigManager.LoadConfig<TestConfig>(appName, fileName);

            Assert.Equal(config.Theme, loaded.Theme);
            Assert.Equal(config.FontSize, loaded.FontSize);
        }
        finally
        {
            // Cleanup
            var dir = XdgPaths.GetConfigDir(appName);
            var path = Path.Combine(dir, fileName);
            if (File.Exists(path))
                File.Delete(path);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void ConfigManager_LoadNonExistent_ShouldCreateDefaultAndReturn()
    {
        var appName = "TestConfigNew_" + Guid.NewGuid().ToString("N");
        var fileName = "new_config.yaml";

        try
        {
            var config = ConfigManager.LoadConfig<TestConfig>(appName, fileName);
            Assert.NotNull(config);
            Assert.Equal("Light", config.Theme); // Default value from class
            Assert.Equal(12, config.FontSize); // Default value from class
        }
        finally
        {
            var dir = XdgPaths.GetConfigDir(appName);
            var path = Path.Combine(dir, fileName);
            if (File.Exists(path))
                File.Delete(path);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void ConfigManager_LoadInvalidYaml_ShouldReturnDefault()
    {
        var appName = "TestConfigBad_" + Guid.NewGuid().ToString("N");
        var fileName = "bad.yaml";

        try
        {
            var dir = XdgPaths.GetConfigDir(appName);
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, fileName), "{{{invalid yaml}}}");

            var config = ConfigManager.LoadConfig<TestConfig>(appName, fileName);
            Assert.NotNull(config);
            Assert.Equal("Light", config.Theme);
        }
        finally
        {
            var dir = XdgPaths.GetConfigDir(appName);
            var path = Path.Combine(dir, fileName);
            if (File.Exists(path))
                File.Delete(path);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void UtilityResult_ShouldStoreProperties()
    {
        var result = new UtilityResult(true, "Success message", "Error details");
        Assert.True(result.IsSuccess);
        Assert.Equal("Success message", result.Message);
        Assert.Equal("Error details", result.ErrorDetails);

        var failResult = new UtilityResult(false, "Failed", null);
        Assert.False(failResult.IsSuccess);
        Assert.Equal("Failed", failResult.Message);
        Assert.Null(failResult.ErrorDetails);
    }

    [Fact]
    public void BadgeStatus_ShouldHaveAllValues()
    {
        Assert.True(Enum.IsDefined(typeof(BadgeStatus), BadgeStatus.Info));
        Assert.True(Enum.IsDefined(typeof(BadgeStatus), BadgeStatus.Success));
        Assert.True(Enum.IsDefined(typeof(BadgeStatus), BadgeStatus.Warning));
        Assert.True(Enum.IsDefined(typeof(BadgeStatus), BadgeStatus.Error));
    }

    [Theory]
    [InlineData("Info", BadgeStatus.Info)]
    [InlineData("Success", BadgeStatus.Success)]
    [InlineData("Warning", BadgeStatus.Warning)]
    [InlineData("Error", BadgeStatus.Error)]
    public void BadgeStatus_ShouldParseCorrectly(string name, BadgeStatus expected)
    {
        var parsed = Enum.Parse<BadgeStatus>(name);
        Assert.Equal(expected, parsed);
    }

    private class TestConfig
    {
        public string Theme { get; set; } = "Light";
        public int FontSize { get; set; } = 12;
    }
}
