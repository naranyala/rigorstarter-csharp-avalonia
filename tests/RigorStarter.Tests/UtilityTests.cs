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
}
