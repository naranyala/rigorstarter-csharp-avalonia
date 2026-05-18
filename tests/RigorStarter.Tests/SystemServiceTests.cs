using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using Xunit;

namespace RigorStarter.Tests;

public class SystemServiceTests
{
    private readonly SystemService _service;

    public SystemServiceTests()
    {
        _service = new SystemService();
    }

    [Fact]
    public void GetNetworkSummary_ShouldContainInterfaceInfo()
    {
        var result = _service.GetNetworkSummary();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        // Should mention interfaces or a fallback message
        Assert.True(
            result.Contains("Interface") || result.Contains("No active"),
            $"Unexpected network summary: {result}"
        );
    }

    [Fact]
    public void GetDiskSummary_ShouldContainDriveInfo()
    {
        var result = _service.GetDiskSummary();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        // Should mention drives or "No ready drives" message
        Assert.True(
            result.Contains("Drive") || result.Contains("No ready drives"),
            $"Unexpected disk summary: {result}"
        );
    }

    [Fact]
    public void GetSystemSummary_ShouldContainHostname()
    {
        var result = _service.GetSystemSummary();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains("Hostname:", result);
    }

    [Fact]
    public void GetTopProcesses_ShouldListTopProcesses()
    {
        var result = _service.GetTopProcesses();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains("Top 10 Processes", result);
    }

    [Fact]
    public void GetMemorySummary_ShouldReturnMemoryInfo()
    {
        var result = _service.GetMemorySummary();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains("Memory", result, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetCpuSummary_ShouldReturnCpuInfo()
    {
        var result = _service.GetCpuSummary();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains("CPU", result, System.StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(nameof(SystemService.GetNetworkSummary))]
    [InlineData(nameof(SystemService.GetDiskSummary))]
    [InlineData(nameof(SystemService.GetSystemSummary))]
    [InlineData(nameof(SystemService.GetTopProcesses))]
    [InlineData(nameof(SystemService.GetMemorySummary))]
    [InlineData(nameof(SystemService.GetCpuSummary))]
    public void AllSummaries_ShouldReturnNonEmpty(string methodName)
    {
        var method = typeof(SystemService).GetMethod(methodName);
        Assert.NotNull(method);

        var result = method.Invoke(_service, null);
        Assert.NotNull(result);
        Assert.NotEmpty((string)result);
    }
}
