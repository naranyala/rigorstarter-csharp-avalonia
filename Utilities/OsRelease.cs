using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RigorStarter.Utilities;

public record DistroInfo(string Name, string Version, string Id, string PrettyName);

public static class OsRelease
{
    private const string OsReleasePath = "/etc/os-release";

    public static DistroInfo GetDistroInfo()
    {
        if (!File.Exists(OsReleasePath))
        {
            return new DistroInfo("Unknown", "Unknown", "unknown", "Unknown Linux");
        }

        var lines = File.ReadAllLines(OsReleasePath);
        var data = new Dictionary<string, string>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;
            var parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                var key = parts[0];
                var value = parts[1].Trim('"');
                data[key] = value;
            }
        }

        return new DistroInfo(
            data.GetValueOrDefault("NAME", "Unknown"),
            data.GetValueOrDefault("VERSION", "Unknown"),
            data.GetValueOrDefault("ID", "unknown"),
            data.GetValueOrDefault("PRETTY_NAME", "Unknown Linux")
        );
    }
}
