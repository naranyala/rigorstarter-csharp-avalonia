using System.Runtime.InteropServices;
using System.Text;

namespace RigorStarter.Shared.Native;

/// <summary>
/// Low-level P/Invoke definitions.
/// These should be kept as thin wrappers around the C functions.
/// </summary>
internal static class NativeMethods
{
    private const string LibC = "libc";

    [DllImport(LibC, SetLastError = true)]
    public static extern int sysinfo(out SysInfo info);

    [DllImport(LibC, SetLastError = true)]
    public static extern int gethostname(StringBuilder name, uint len);

    [DllImport(LibC, SetLastError = true)]
    public static extern int uname(out Utsname buf);

    [DllImport(LibC, SetLastError = true)]
    public static extern int getloadavg(double[] loadavg, int nelements);
}
