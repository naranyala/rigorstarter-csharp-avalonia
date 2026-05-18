using System;
using System.Runtime.InteropServices;
using System.Text;
using RigorStarter.Shared.Native;

namespace RigorStarter.Shared.Native;

/// <summary>
/// A managed wrapper around native C memory.
/// Implements IDisposable to ensure malloc'd memory is freed via free().
/// </summary>
public unsafe class NativeBuffer : IDisposable
{
    public IntPtr Pointer { get; private set; }
    public uint Size { get; private set; }
    private bool _disposed = false;

    public NativeBuffer(uint size)
    {
        Size = size;
        Pointer = StdLibMethods.malloc((UIntPtr)size);
        if (Pointer == IntPtr.Zero)
            throw new OutOfMemoryException("Native malloc failed to allocate memory.");
    }

    public void WriteByte(uint offset, byte value)
    {
        if (offset >= Size)
            throw new IndexOutOfRangeException();
        byte* p = (byte*)Pointer + (int)offset;
        *p = value;
    }

    public byte ReadByte(uint offset)
    {
        if (offset >= Size)
            throw new IndexOutOfRangeException();
        byte* p = (byte*)Pointer + (int)offset;
        return *p;
    }

    public void Fill(byte value)
    {
        StdLibMethods.memset(Pointer.ToPointer(), value, (UIntPtr)Size);
    }

    public void CopyFrom(NativeBuffer other, uint offset = 0)
    {
        if (offset + other.Size > this.Size)
            throw new ArgumentOutOfRangeException("Buffer too small");
        StdLibMethods.memcpy((byte*)Pointer + offset, (byte*)other.Pointer, (UIntPtr)other.Size);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            StdLibMethods.free(Pointer);
            Pointer = IntPtr.Zero;
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    ~NativeBuffer() => Dispose();
}

public static class StdLibBridge
{
    public static (bool Success, UIntPtr Length) GetStringLength(string s)
    {
        if (s == null)
            return (false, 0);
        return (true, StdLibMethods.strlen(s));
    }

    public static IntPtr DuplicateString(string s)
    {
        return StdLibMethods.strdup(s);
    }

    public static void FreeString(IntPtr ptr)
    {
        StdLibMethods.free(ptr);
    }
}
