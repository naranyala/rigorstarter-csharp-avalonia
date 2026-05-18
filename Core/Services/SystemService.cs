using System;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Utilities;

namespace RigorStarter.Core.Services;

public class SystemService : ISystemService
{
    public string GetNetworkSummary() => NetworkUtility.GetNetworkSummary().Message;

    public string GetDiskSummary() => DiskUtility.GetDiskSummary().Message;

    public string GetSystemSummary() => SystemInfoUtility.GetSystemSummary().Message;

    public string GetTopProcesses() => ProcessUtility.GetTopProcesses().Message;

    public string GetMemorySummary() => MemoryUtility.GetMemorySummary().Message;

    public string GetCpuSummary() => CpuUtility.GetCpuSummary().Message;
}
