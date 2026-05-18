using System.Linq;
using RigorStarter.Shared.Native;
using Xunit;

namespace RigorStarter.Tests;

public class NativeTests
{
    [Fact]
    public void LinuxNativeBridge_GetHostname_ShouldReturnValidName()
    {
        var (success, result) = LinuxNativeBridge.GetHostname();
        Assert.True(success);
        Assert.False(string.IsNullOrWhiteSpace(result));
    }

    [Fact]
    public void LinuxNativeBridge_GetSysInfo_ShouldReturnValidData()
    {
        var (success, info) = LinuxNativeBridge.GetSysInfo();
        Assert.True(success);
        Assert.True(info.totalram > 0);
    }

    [Fact]
    public void LinuxNativeBridge_GetKernelVersion_ShouldReturnValidVersion()
    {
        var (success, version) = LinuxNativeBridge.GetKernelVersion();
        Assert.True(success);
        Assert.False(string.IsNullOrWhiteSpace(version));
    }

    [Fact]
    public void LinuxNativeBridge_GetLoadAverage_ShouldReturnThreeValues()
    {
        var (success, loads) = LinuxNativeBridge.GetLoadAverage();
        Assert.True(success);
        Assert.Equal(3, loads.Length);
    }
}
