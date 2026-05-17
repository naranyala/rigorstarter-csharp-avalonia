using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;

namespace RigorStarter.Utilities;

public static class NetworkUtility
{
    public static string GetNetworkSummary()
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
            
            return summary.Count > 0 
                ? string.Join("\n", summary) 
                : "No active network interfaces found.";
        }
        catch (Exception ex)
        {
            return $"Error retrieving network info: {ex.Message}";
        }
    }
}
