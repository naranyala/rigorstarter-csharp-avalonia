using System;
using System.IO;

namespace RigorStarter.Utilities;

public static class XdgPaths
{
    public static string GetConfigDir(string appName)
    {
        var path =
            Environment.GetEnvironmentVariable("XDG_CONFIG_HOME")
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config"
            );
        return Path.Combine(path, appName);
    }

    public static string GetDataDir(string appName)
    {
        var path =
            Environment.GetEnvironmentVariable("XDG_DATA_HOME")
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".local",
                "share"
            );
        return Path.Combine(path, appName);
    }

    public static string GetCacheDir(string appName)
    {
        var path =
            Environment.GetEnvironmentVariable("XDG_CACHE_HOME")
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".cache"
            );
        return Path.Combine(path, appName);
    }

    public static void EnsureDirectories(string appName)
    {
        Directory.CreateDirectory(GetConfigDir(appName));
        Directory.CreateDirectory(GetDataDir(appName));
        Directory.CreateDirectory(GetCacheDir(appName));
    }
}
