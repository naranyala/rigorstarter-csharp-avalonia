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
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public unsafe struct Utsname
{
    public fixed byte sysname[65];
    public fixed byte nodename[65];
    public fixed byte release[65];
    public fixed byte version[65];
    public fixed byte machine[65];
}
