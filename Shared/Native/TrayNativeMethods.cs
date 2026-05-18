using System;
using System.Runtime.InteropServices;

namespace RigorStarter.Shared.Native;

/// <summary>
/// P/Invoke signatures for libappindicator.
/// </summary>
internal static class TrayNativeMethods
{
    private const string LibAppIndicator = "libappindicator-3";

    [DllImport(LibAppIndicator)]
    public static extern IntPtr app_indicator_new(string id, string icon_name, string category);

    [DllImport(LibAppIndicator)]
    public static extern void app_indicator_set_status(IntPtr indicator, int status);

    [DllImport(LibAppIndicator)]
    public static extern void app_indicator_set_index(IntPtr indicator, int index);
}
