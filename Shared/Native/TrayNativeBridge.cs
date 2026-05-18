using System;
using RigorStarter.Shared.Native;

namespace RigorStarter.Shared.Native;

public static class TrayNativeBridge
{
    private static IntPtr _indicatorHandle = IntPtr.Zero;

    public static bool Initialize(string id, string icon, string tooltip)
    {
        try
        {
            _indicatorHandle = TrayNativeMethods.app_indicator_new(id, icon, "Application");
            return _indicatorHandle != IntPtr.Zero;
        }
        catch
        {
            return false;
        }
    }

    public static void SetStatus(int status)
    {
        if (_indicatorHandle != IntPtr.Zero)
        {
            TrayNativeMethods.app_indicator_set_status(_indicatorHandle, status);
        }
    }

    public static void SetVisibility(bool visible)
    {
        SetStatus(visible ? 1 : 0);
    }
}
