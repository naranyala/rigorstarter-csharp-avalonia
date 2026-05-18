using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using Xunit;

namespace RigorStarter.Tests;

public class SystemServiceTests
{
    [Fact]
    public void SystemService_ShouldReturnNonNullStrings()
    {
        // Arrange
        var service = new SystemService();

        // Act & Assert
        Assert.NotNull(service.GetNetworkSummary());
        Assert.NotNull(service.GetDiskSummary());
        Assert.NotNull(service.GetSystemSummary());
        Assert.NotNull(service.GetTopProcesses());
        Assert.NotNull(service.GetMemorySummary());
        Assert.NotNull(service.GetCpuSummary());
    }
}
