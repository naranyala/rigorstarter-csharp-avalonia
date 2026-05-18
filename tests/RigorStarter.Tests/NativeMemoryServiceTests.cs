using System;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.Shared.Native;
using Xunit;

namespace RigorStarter.Tests;

public class NativeMemoryServiceTests
{
    private readonly NativeMemoryService _service;

    public NativeMemoryServiceTests()
    {
        _service = new NativeMemoryService();
    }

    [Fact]
    public void Allocate_ShouldCreateNativeBuffer()
    {
        using var buffer = _service.Allocate(1024);
        Assert.NotNull(buffer);
        Assert.Equal(1024u, buffer.Size);
        Assert.NotEqual(IntPtr.Zero, buffer.Pointer);
    }

    [Fact]
    public void Allocate_SmallBuffer_ShouldWork()
    {
        using var buffer = _service.Allocate(1);
        Assert.Equal(1u, buffer.Size);
        Assert.NotEqual(IntPtr.Zero, buffer.Pointer);
    }

    [Fact]
    public void Allocate_LargeBuffer_ShouldWork()
    {
        using var buffer = _service.Allocate(1024 * 1024); // 1 MB
        Assert.Equal(1024u * 1024, buffer.Size);
        Assert.NotEqual(IntPtr.Zero, buffer.Pointer);
    }

    [Fact]
    public void GetNativeStringLength_ShouldReturnLength()
    {
        var result = _service.GetNativeStringLength("hello");
        Assert.Contains("Native Length: 5", result);
    }

    [Fact]
    public void GetNativeStringLength_EmptyString_ShouldReturnZero()
    {
        var result = _service.GetNativeStringLength(string.Empty);
        Assert.Contains("Native Length: 0", result);
    }

    [Fact]
    public void GetNativeStringLength_Null_ShouldReturnError()
    {
        var result = _service.GetNativeStringLength(null!);
        Assert.Contains("Error", result);
    }
}
