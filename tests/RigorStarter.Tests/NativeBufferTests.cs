using System;
using RigorStarter.Shared.Native;
using Xunit;

namespace RigorStarter.Tests;

public class NativeBufferTests
{
    [Fact]
    public void Allocate_ShouldCreateBufferOfCorrectSize()
    {
        uint size = 1024;
        using var buffer = new NativeBuffer(size);
        Assert.Equal(size, buffer.Size);
        Assert.NotEqual(IntPtr.Zero, buffer.Pointer);
    }

    [Fact]
    public void WriteAndRead_ShouldReturnCorrectValue()
    {
        using var buffer = new NativeBuffer(10);
        byte testValue = 0xAA;
        uint offset = 5;

        buffer.WriteByte(offset, testValue);
        var result = buffer.ReadByte(offset);

        Assert.Equal(testValue, result);
    }

    [Fact]
    public void Write_OutOfBounds_ShouldThrowException()
    {
        using var buffer = new NativeBuffer(10);
        Assert.Throws<IndexOutOfRangeException>(() => buffer.WriteByte(10, 0xFF));
        Assert.Throws<IndexOutOfRangeException>(() => buffer.WriteByte(100, 0xFF));
    }

    [Fact]
    public void Read_OutOfBounds_ShouldThrowException()
    {
        using var buffer = new NativeBuffer(10);
        Assert.Throws<IndexOutOfRangeException>(() => buffer.ReadByte(10));
    }

    [Fact]
    public void Fill_ShouldSetAllBytesToValue()
    {
        using var buffer = new NativeBuffer(100);
        byte fillValue = 0xCC;

        buffer.Fill(fillValue);

        for (uint i = 0; i < 100; i++)
        {
            Assert.Equal(fillValue, buffer.ReadByte(i));
        }
    }

    [Fact]
    public void CopyFrom_ShouldCorrectlyTransferData()
    {
        using var src = new NativeBuffer(10);
        using var dest = new NativeBuffer(20);

        // Fill src with values 0..9
        for (uint i = 0; i < 10; i++)
            src.WriteByte(i, (byte)i);

        // Copy src to dest at offset 5
        dest.CopyFrom(src, 5);

        for (uint i = 0; i < 10; i++)
        {
            Assert.Equal((byte)i, dest.ReadByte(5 + i));
        }
    }

    [Fact]
    public void CopyFrom_OutOfBounds_ShouldThrowException()
    {
        using var src = new NativeBuffer(10);
        using var dest = new NativeBuffer(10);

        // Attempt to copy 10 bytes at offset 5 (requires 15 bytes space)
        Assert.Throws<ArgumentOutOfRangeException>(() => dest.CopyFrom(src, 5));
    }

    [Fact]
    public void Dispose_ShouldNotCrashOnMultipleCalls()
    {
        var buffer = new NativeBuffer(10);
        buffer.Dispose();

        // Second call should be handled gracefully by the internal _disposed flag
        var exception = Record.Exception(() => buffer.Dispose());
        Assert.Null(exception);
    }
}
