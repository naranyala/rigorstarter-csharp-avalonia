using System.Runtime.InteropServices;

namespace RigorStarter.Shared.Native;

/// <summary>
/// C-compatible struct for sysinfo (from <sys/sysinfo.h>)
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct SysInfo
{
    public ulong uptime;
    public fixed ulong loads[3];
    public ulong totalram;
    public ulong freeram;
    public uint sharedram;
    public uint bufferram;
    public uint totalswap;
    public uint freeswap;
    public short procs;
    public short pad;
    public uint totalhigh;
    public uint freehigh;
    public uint mem_unit;
    public fixed char _s[20];
}

/// <summary>
/// C-compatible struct for utsname (from <sys/utsname.h>)
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Utsname
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)]
    public byte[] sysname;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)]
    public byte[] nodename;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)]
    public byte[] release;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)]
    public byte[] version;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)]
    public byte[] machine;
}
