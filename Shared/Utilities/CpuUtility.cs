using System;
using RigorStarter.Shared.Models;
using RigorStarter.Shared.Utilities;

namespace RigorStarter.Shared.Utilities;

public static class CpuUtility
{
    public static UtilityResult GetCpuSummary()
    {
        try
        {
            // In a real app, we'd use platform-specific APIs
            return new UtilityResult(true, "CPU Usage: 12% (Average)", null);
        }
        catch (Exception ex)
        {
            return new UtilityResult(false, $"Error retrieving CPU info: {ex.Message}", null);
        }
    }
}
