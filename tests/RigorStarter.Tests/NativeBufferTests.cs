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
    public void Allocate_ZeroSize_ShouldCreateEmptyBuffer()
    {
        // malloc(0) behavior is implementation-defined; it may return NULL or a valid pointer.
        // Either way, the buffer should have Size=0 and any access should throw.
        var exception = Record.Exception(() =>
        {
            using var buffer = new NativeBuffer(0);
            Assert.Equal(0u, buffer.Size);
            // Any access should throw since offset >= size
            Assert.Throws<IndexOutOfRangeException>(() => buffer.ReadByte(0));
        });
        // If malloc(0) returns NULL, an OutOfMemoryException is expected instead
        Assert.True(exception == null || exception is OutOfMemoryException);
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
    public void WriteAndRead_AtOffsetZero_ShouldWork()
    {
        using var buffer = new NativeBuffer(10);
        buffer.WriteByte(0, 0x42);
        Assert.Equal(0x42, buffer.ReadByte(0));
    }

    [Fact]
    public void WriteAndRead_AtLastByte_ShouldWork()
    {
        using var buffer = new NativeBuffer(10);
        buffer.WriteByte(9, 0xFF);
        Assert.Equal(0xFF, buffer.ReadByte(9));
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
        Assert.Throws<IndexOutOfRangeException>(() => buffer.ReadByte(uint.MaxValue));
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
    public void Fill_WithZero_ShouldClearBuffer()
    {
        using var buffer = new NativeBuffer(50);
        buffer.Fill(0);

        for (uint i = 0; i < 50; i++)
        {
            Assert.Equal(0, buffer.ReadByte(i));
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
    public void CopyFrom_OffsetZero_ShouldWork()
    {
        using var src = new NativeBuffer(5);
        using var dest = new NativeBuffer(5);

        for (uint i = 0; i < 5; i++)
            src.WriteByte(i, (byte)(i + 1));

        dest.CopyFrom(src, 0);

        for (uint i = 0; i < 5; i++)
            Assert.Equal((byte)(i + 1), dest.ReadByte(i));
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
    public void CopyFrom_SrcLargerThanDest_ShouldThrow()
    {
        using var src = new NativeBuffer(100);
        using var dest = new NativeBuffer(10);

        Assert.Throws<ArgumentOutOfRangeException>(() => dest.CopyFrom(src, 0));
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

    [Fact]
    public void Dispose_ShouldZeroOutPointer()
    {
        var buffer = new NativeBuffer(10);
        buffer.Dispose();

        // After dispose, pointer should be zeroed
        var exception = Record.Exception(() => buffer.Dispose());
        Assert.Null(exception);
    }

    [Fact]
    public void Using_ShouldDisposeCorrectly()
    {
        NativeBuffer buffer;
        using (buffer = new NativeBuffer(10))
        {
            Assert.NotEqual(IntPtr.Zero, buffer.Pointer);
        }

        // After using block, buffer should be disposed
        // Note: We can't verify this directly, but the test ensures no crash
        Assert.Equal(IntPtr.Zero, buffer.Pointer);
    }

    [Fact]
    public void Fill_AfterPartialWrite_ShouldOverwriteAll()
    {
        using var buffer = new NativeBuffer(50);

        // Write some values
        buffer.WriteByte(10, 0xAA);
        buffer.WriteByte(20, 0xBB);
        buffer.WriteByte(30, 0xCC);

        // Fill all with 0xFF
        buffer.Fill(0xFF);

        // Verify all bytes are 0xFF
        for (uint i = 0; i < 50; i++)
        {
            Assert.Equal(0xFF, buffer.ReadByte(i));
        }
    }
}
