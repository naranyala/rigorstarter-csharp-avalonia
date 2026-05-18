using System;
using System.Runtime.InteropServices;
using RigorStarter.Shared.Models;

namespace RigorStarter.Shared.Utilities;

public static class SystemInfoUtility
{
    public static UtilityResult GetSystemSummary()
    {
        try
        {
            string info =
                $"OS: {RuntimeInformation.OSDescription}\n"
                + $"Architecture: {RuntimeInformation.OSArchitecture}\n"
                + $"Framework: {RuntimeInformation.FrameworkDescription}\n"
                + $"Machine Name: {Environment.MachineName}\n"
                + $"Processor Count: {Environment.ProcessorCount}\n"
                + $"User: {Environment.UserName}";
            return new UtilityResult(true, info);
        }
        catch (Exception ex)
        {
            return new UtilityResult(false, "Failed to retrieve system info", ex.Message);
        }
    }
}
