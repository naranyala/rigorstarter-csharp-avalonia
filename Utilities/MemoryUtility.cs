using System;
using RigorStarter.Utilities;

namespace RigorStarter.Utilities;

public static class MemoryUtility
{
    public static UtilityResult GetMemorySummary()
    {
        try
        {
            // In a real app, we'd use platform-specific APIs
            return new UtilityResult(true, "Memory Usage: 4.2GB / 16GB (26%)", null);
        }
        catch (Exception ex)
        {
            return new UtilityResult(false, $"Error retrieving memory info: {ex.Message}", null);
        }
    }
}
