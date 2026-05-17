using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace RigorStarter.Utilities;

public static class NetworkUtility
{
    public static UtilityResult GetNetworkSummary()
    {
        try
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            var summary = new List<string>();

            foreach (var ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    var stats = ni.GetIPStatistics();
                    summary.Add($"Interface: {ni.Name}");
                    summary.Add($"  Status: {ni.OperationalStatus}");
                    summary.Add($"  Speed: {ni.Speed / 1_000_000} Mbps");
                    summary.Add($"  Received Bytes: {stats.BytesReceived}");
                    summary.Add($"  Sent Bytes: {stats.BytesSent}");
                }
            }

            string result =
                summary.Count > 0
                    ? string.Join("\n", summary)
                    : "No active network interfaces found.";

            return new UtilityResult(true, result);
        }
        catch (Exception ex)
        {
            return new UtilityResult(false, "Failed to retrieve network information", ex.Message);
        }
    }
}
