using System;
using System.Threading.Tasks;
using RigorStarter.Utilities;
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
    }

    [Fact]
    public void SystemHub_ShouldRouteToProcFS()
    {
        var ram = SystemHub.TotalRam;
        Assert.True(ram > 0);
    }

    [Fact]
    public async Task SystemHub_ShouldRouteToPackageManager()
    {
        var pkg = await SystemHub.GetPkgManager();
        // We don't assert a specific manager because it depends on the host OS,
        // but we ensure it returns a valid enum value.
        Assert.True(Enum.IsDefined(typeof(PackageManagerType), pkg));
    }
}
