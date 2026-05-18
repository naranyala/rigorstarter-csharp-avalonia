namespace RigorStarter.Core.Interfaces;

public interface ISystemService
{
    string GetNetworkSummary();
    string GetDiskSummary();
    string GetSystemSummary();
    string GetTopProcesses();
    string GetMemorySummary();
    string GetCpuSummary();
}
