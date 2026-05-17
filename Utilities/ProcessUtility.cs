using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RigorStarter.Utilities;

public static class ProcessUtility
{
    public static UtilityResult GetTopProcesses()
    {
        try
        {
            var processes = Process
                .GetProcesses()
                .OrderByDescending(p => p.WorkingSet64)
                .Take(10)
                .Select(p => $"{p.ProcessName} ({p.WorkingSet64 / (1024 * 1024)} MB)")
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Top 10 Processes by Memory Usage:");
            foreach (var proc in processes)
            {
                sb.AppendLine($"- {proc}");
            }

            return new UtilityResult(true, sb.ToString().TrimEnd());
        }
        catch (Exception ex)
        {
            return new UtilityResult(false, "Failed to retrieve process list", ex.Message);
        }
    }
}
