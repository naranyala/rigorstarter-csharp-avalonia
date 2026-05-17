using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RigorStarter.Utilities;

public record ShellResult(int ExitCode, string StandardOutput, string StandardError);

public static class LinuxShell
{
    public static async Task<ShellResult> ExecuteAsync(
        string command,
        string arguments = "",
        int timeoutMilliseconds = 5000
    )
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = startInfo };

        try
        {
            process.Start();

            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            if (
                await Task.WhenAny(Task.Delay(timeoutMilliseconds), process.WaitForExitAsync())
                == Task.Delay(timeoutMilliseconds)
            )
            {
                process.Kill();
                return new ShellResult(-1, "", "Command timed out");
            }

            return new ShellResult(process.ExitCode, await outputTask, await errorTask);
        }
        catch (Exception ex)
        {
            return new ShellResult(-1, "", ex.Message);
        }
    }
}
