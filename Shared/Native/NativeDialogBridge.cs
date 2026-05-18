using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RigorStarter.Shared.Native;

/// <summary>
/// Bridge to native OS dialog providers.
/// This uses the Process-Bridge pattern to trigger native system dialogs.
/// </summary>
public static class NativeDialogBridge
{
    private static async Task<string?> ExecuteNativeDialogAsync(string args)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "zenity",
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            using var process = Process.Start(startInfo);
            if (process == null)
                return null;

            string result = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return process.ExitCode == 0 ? result.Trim() : null;
        }
        catch
        {
            return null;
        }
    }

    public static async Task<string?> OpenFileAsync(string title, string filter)
    {
        // Zenity file-selection dialog
        string args = $"--file-selection --title=\"{title}\"";
        return await ExecuteNativeDialogAsync(args);
    }

    public static async Task<string?> SaveFileAsync(string title, string filter)
    {
        // Zenity save dialog
        string args = $"--file-selection --save --title=\"{title}\"";
        return await ExecuteNativeDialogAsync(args);
    }

    public static async Task<bool> ConfirmAsync(string title, string message)
    {
        // Zenity question dialog
        string args = $"--question --title=\"{title}\" --text=\"{message}\"";
        var result = await ExecuteNativeDialogAsync(args);
        return result == "true" || result == "Yes";
    }

    public static async Task ShowMessageAsync(string title, string message)
    {
        // Zenity info dialog
        string args = $"--info --title=\"{title}\" --text=\"{message}\"";
        await ExecuteNativeDialogAsync(args);
    }
}
