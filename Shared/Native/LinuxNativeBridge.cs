using System;
using System.Runtime.InteropServices;
using System.Text;
using RigorStarter.Shared.Native;

namespace RigorStarter.Shared.Native;

/// <summary>
/// Mid-level abstraction that handles marshaling and error checks.
/// This prevents native-related crashes from leaking into the Core layer.
/// </summary>
public static class LinuxNativeBridge
{
    public static (bool Success, string Result) GetHostname()
    {
        try
        {
            var sb = new StringBuilder(256);
            int result = NativeMethods.gethostname(sb, (uint)sb.Capacity);
            return result == 0 ? (true, sb.ToString()) : (false, "Failed to get hostname");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public static (bool Success, SysInfo Info) GetSysInfo()
    {
        try
        {
            int result = NativeMethods.sysinfo(out SysInfo info);
            return result == 0 ? (true, info) : (false, default);
        }
        catch (Exception)
        {
            return (false, default);
        }
    }

    public static (bool Success, string KernelVersion) GetKernelVersion()
    {
        try
        {
            int result = NativeMethods.uname(out Utsname buf);
            if (result != 0)
                return (false, string.Empty);

            unsafe
            {
                // buf.release is a fixed byte buffer. We can access it directly as a pointer.
                byte* pRelease = buf.release;
                return (true, Marshal.PtrToStringAnsi((IntPtr)pRelease) ?? string.Empty);
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public static (bool Success, double[] Loads) GetLoadAverage()
    {
        try
        {
            double[] loads = new double[3];
            int result = NativeMethods.getloadavg(loads, 3);
            return result == -1 ? (false, Array.Empty<double>()) : (true, loads);
        }
        catch (Exception)
        {
            return (false, Array.Empty<double>());
        }
    }
}
