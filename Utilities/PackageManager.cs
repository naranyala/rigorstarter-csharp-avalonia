using System;
using System.Threading.Tasks;

namespace RigorStarter.Utilities;

public enum PackageManagerType
{
    Apt,
    Dnf,
    Pacman,
    Zypper,
    Unknown,
}

public static class PackageManager
{
    public static async Task<PackageManagerType> DetectPackageManager()
    {
        if (await CommandExists("apt-get"))
            return PackageManagerType.Apt;
        if (await CommandExists("dnf"))
            return PackageManagerType.Dnf;
        if (await CommandExists("pacman"))
            return PackageManagerType.Pacman;
        if (await CommandExists("zypper"))
            return PackageManagerType.Zypper;

        return PackageManagerType.Unknown;
    }

    private static async Task<bool> CommandExists(string command)
    {
        var result = await LinuxShell.ExecuteAsync("which", command);
        return result.ExitCode == 0;
    }
}
