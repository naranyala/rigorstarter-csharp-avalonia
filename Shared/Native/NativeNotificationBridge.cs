using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RigorStarter.Shared.Native;

/// <summary>
/// Bridge to native OS notification system.
/// Uses the native process-bridge pattern to interact with the system notification daemon.
/// </summary>
public static class NativeNotificationBridge
{
    public static async Task<bool> SendNotificationAsync(
        string title,
        string message,
        string urgency
    )
    {
        try
        {
            // 'notify-send' is the native Linux standard for triggering system notifications
            var startInfo = new ProcessStartInfo
            {
                FileName = "notify-send",
                Arguments = $"-u {urgency} \"{title}\" \"{message}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            using var process = Process.Start(startInfo);
            if (process == null)
                return false;

            await process.WaitForExitAsync();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
