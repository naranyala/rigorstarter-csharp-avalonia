using System.Threading.Tasks;

namespace RigorStarter.Utilities;

public static class SystemdManager
{
    public static async Task<bool> IsServiceActive(string serviceName)
    {
        var result = await LinuxShell.ExecuteAsync("systemctl", $"is-active {serviceName}");
        return result.ExitCode == 0 && result.StandardOutput.Trim() == "active";
    }

    public static async Task<int> RestartService(string serviceName)
    {
        // Note: This usually requires sudo/root privileges
        var result = await LinuxShell.ExecuteAsync("systemctl", $"restart {serviceName}");
        return result.ExitCode;
    }

    public static async Task<string> GetServiceStatus(string serviceName)
    {
        var result = await LinuxShell.ExecuteAsync("systemctl", $"status {serviceName}");
        return result.StandardOutput;
    }
}
