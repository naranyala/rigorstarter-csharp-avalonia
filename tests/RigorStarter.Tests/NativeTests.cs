using System;
using System.Runtime.InteropServices;
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
        Assert.True(success, $"Expected success but got: {version}");
        Assert.False(string.IsNullOrWhiteSpace(version));
    }

    [Fact]
    public void LinuxNativeBridge_GetLoadAverage_ShouldReturnThreeValues()
    {
        var (success, loads) = LinuxNativeBridge.GetLoadAverage();
        Assert.True(success);
        Assert.Equal(3, loads.Length);
    }

    [Fact]
    public void StdLibBridge_GetStringLength_ShouldReturnCorrectLength()
    {
        var (success, length) = StdLibBridge.GetStringLength("hello");
        Assert.True(success);
        Assert.Equal(5u, (uint)length);

        (success, length) = StdLibBridge.GetStringLength(string.Empty);
        Assert.True(success);
        Assert.Equal(0u, (uint)length);
    }

    [Fact]
    public void StdLibBridge_GetStringLength_Null_ShouldReturnFalse()
    {
        var (success, length) = StdLibBridge.GetStringLength(null!);
        Assert.False(success);
        Assert.Equal(0u, (uint)length);
    }

    [Fact]
    public void StdLibBridge_DuplicateString_ShouldCreateCopy()
    {
        string original = "test_string";
        IntPtr ptr = StdLibBridge.DuplicateString(original);

        try
        {
            Assert.NotEqual(IntPtr.Zero, ptr);
            string? result = Marshal.PtrToStringAnsi(ptr);
            Assert.Equal(original, result);
        }
        finally
        {
            StdLibBridge.FreeString(ptr);
        }
    }

    [Fact]
    public void StdLibBridge_DuplicateString_Empty_ShouldReturnValidPtr()
    {
        IntPtr ptr = StdLibBridge.DuplicateString(string.Empty);

        try
        {
            Assert.NotEqual(IntPtr.Zero, ptr);
            string? result = Marshal.PtrToStringAnsi(ptr);
            Assert.Equal(string.Empty, result);
        }
        finally
        {
            StdLibBridge.FreeString(ptr);
        }
    }

    [Fact]
    public void StdLibBridge_FreeString_Null_ShouldNotCrash()
    {
        var exception = Record.Exception(() => StdLibBridge.FreeString(IntPtr.Zero));
        Assert.Null(exception);
    }

    [Fact]
    public void NativeBuffer_AllocateAndFree_ShouldNotLeak()
    {
        using var buffer = new NativeBuffer(1024);
        Assert.NotEqual(IntPtr.Zero, buffer.Pointer);
        Assert.Equal(1024u, buffer.Size);
    }
}
