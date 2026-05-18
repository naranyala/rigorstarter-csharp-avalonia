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
            int size = Marshal.SizeOf<Utsname>();
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                int result = NativeMethods.uname(ptr);
                if (result != 0)
                    return (false, string.Empty);

                Utsname buf = Marshal.PtrToStructure<Utsname>(ptr);
                string release =
                    buf.release != null
                        ? System.Text.Encoding.ASCII.GetString(buf.release).TrimEnd('\0')
                        : string.Empty;
                return (true, release);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
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
