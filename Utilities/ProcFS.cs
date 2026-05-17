using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RigorStarter.Utilities;

public static class ProcFS
{
    public static Dictionary<string, string> ReadProcFile(string fileName)
    {
        var results = new Dictionary<string, string>();
        try
        {
            var lines = File.ReadAllLines($"/proc/{fileName}");
            foreach (var line in lines)
            {
                var parts = line.Split(':', 2);
                if (parts.Length == 2)
                {
                    results[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }
        catch (Exception ex)
        {
            results["Error"] = ex.Message;
        }
        return results;
    }

    public static long GetTotalRamKb()
    {
        var mem = ReadProcFile("meminfo");
        if (mem.TryGetValue("MemTotal", out var val))
        {
            var numericPart = new string(val.Where(char.IsDigit).ToArray());
            return long.Parse(numericPart);
        }
        return 0;
    }
}
