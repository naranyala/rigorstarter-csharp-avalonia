using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RigorStarter.Utilities;

public static class DiskUtility
{
    public static UtilityResult GetDiskSummary()
    {
        try
        {
            var sb = new StringBuilder();
            var drives = DriveInfo.GetDrives();
            
            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    sb.AppendLine($"Drive {drive.Name}");
                    sb.AppendLine($"  Type: {drive.DriveType}");
                    sb.AppendLine($"  Total Size: {drive.TotalSize / (1024 * 1024 * 1024):N2} GB");
                    sb.AppendLine($"  Free Space: {drive.AvailableFreeSpace / (1024 * 1024 * 1024):N2} GB");
                    sb.AppendLine($"  Used Space: {(drive.TotalSize - drive.AvailableFreeSpace) / (1024 * 1024 * 1024):N2} GB");
                    sb.AppendLine();
                }
            }
            
            string result = sb.Length > 0 ? sb.ToString().TrimEnd() : "No ready drives found.";
            return new UtilityResult(true, result);
        }
        catch (Exception ex)
        {
            return new UtilityResult(false, "Failed to retrieve disk information", ex.Message);
        }
    }
}
