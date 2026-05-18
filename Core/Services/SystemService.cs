using System;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Native;
using RigorStarter.Shared.Utilities;

namespace RigorStarter.Core.Services;

public class SystemService : ISystemService
{
    public string GetNetworkSummary() => NetworkUtility.GetNetworkSummary().Message;

    public string GetDiskSummary() => DiskUtility.GetDiskSummary().Message;

    public string GetSystemSummary()
    {
        var (hostSuccess, hostname) = LinuxNativeBridge.GetHostname();
        var host = hostSuccess ? hostname : Environment.MachineName;

        var (sysSuccess, sysInfo) = LinuxNativeBridge.GetSysInfo();
        string ramInfo = sysSuccess
            ? $"Total RAM: {sysInfo.totalram / 1024 / 1024} MB"
            : "RAM info unavailable";

        var (kernelSuccess, kernel) = LinuxNativeBridge.GetKernelVersion();
        string kernelInfo = kernelSuccess ? $"Kernel: {kernel}" : "Kernel unknown";

        var (loadSuccess, loads) = LinuxNativeBridge.GetLoadAverage();
        string loadInfo = loadSuccess
            ? $"Load Avg: {loads[0]:F2}, {loads[1]:F2}, {loads[2]:F2}"
            : "Load avg unavailable";

        return $"Hostname: {host}\n{ramInfo}\n{kernelInfo}\n{loadInfo}\n{SystemInfoUtility.GetSystemSummary().Message}";
    }

    public string GetTopProcesses() => ProcessUtility.GetTopProcesses().Message;

    public string GetMemorySummary() => MemoryUtility.GetMemorySummary().Message;

    public string GetCpuSummary() => CpuUtility.GetCpuSummary().Message;
}
