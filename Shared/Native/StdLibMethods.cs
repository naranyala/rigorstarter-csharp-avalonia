using System;
using System.Runtime.InteropServices;

namespace RigorStarter.Shared.Native;

/// <summary>
/// Core libc primitives for a custom Native StdLib.
/// </summary>
internal static unsafe class StdLibMethods
{
    private const string LibC = "libc";

    [DllImport(LibC, SetLastError = true)]
    public static extern IntPtr malloc(UIntPtr size);

    [DllImport(LibC, SetLastError = true)]
    public static extern void free(IntPtr ptr);

    [DllImport(LibC, SetLastError = true)]
    public static extern void* memset(void* dest, int c, UIntPtr count);

    [DllImport(LibC, SetLastError = true)]
    public static extern void* memcpy(void* dest, void* src, UIntPtr count);

    [DllImport(LibC, SetLastError = true)]
    public static extern UIntPtr strlen(string s);

    [DllImport(LibC, SetLastError = true)]
    public static extern IntPtr strdup(string s);
}
