using System;
using System.Threading.Tasks;

namespace RigorStarter.Shared.Utilities;

/// <summary>
/// The SystemHub acts as the primary entry point for the Linux system library,
/// aggregating various specialized modules into a single cohesive API.
/// </summary>
public static class SystemHub
{
    // OS Info
    public static DistroInfo Distro => OsRelease.GetDistroInfo();

    // Path Management
    public static string ConfigPath(string appName) => XdgPaths.GetConfigDir(appName);

    public static string DataPath(string appName) => XdgPaths.GetDataDir(appName);

    public static string CachePath(string appName) => XdgPaths.GetCacheDir(appName);

    // Configuration
    public static T LoadConfig<T>(string fileName)
        where T : new() => ConfigManager.LoadConfig<T>("RigorStarter", fileName);

    public static void SaveConfig<T>(string fileName, T config) =>
        ConfigManager.SaveConfig("RigorStarter", fileName, config);

    // Logging
    public static void LogInfo(string msg) => SystemLogger.Info(msg);

    public static void LogError(string msg, Exception? ex = null) => SystemLogger.Error(msg, ex);

    // System Monitoring
    public static long TotalRam => ProcFS.GetTotalRamKb();
    public static System.Collections.Generic.Dictionary<string, string> MemInfo =>
        ProcFS.ReadProcFile("meminfo");

    // Shell & Automation
    public static Task<ShellResult> Run(string cmd, string args = "") =>
        LinuxShell.ExecuteAsync(cmd, args);

    // Desktop Integration
    public static Task Notify(string title, string msg) =>
        LinuxNotifier.SendNotification(title, msg);

    // DBus Communication
    public static Task<string> GetSystemProperty(
        string dest,
        string path,
        string iface,
        string prop
    ) => DBusService.GetSystemProperty(dest, path, iface, prop);

    // Service Management
    public static Task<bool> IsServiceRunning(string service) =>
        SystemdManager.IsServiceActive(service);

    // Package Management
    public static Task<PackageManagerType> GetPkgManager() => PackageManager.DetectPackageManager();
}
