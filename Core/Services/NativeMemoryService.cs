using System;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Native;

namespace RigorStarter.Core.Services;

public interface INativeMemoryService
{
    NativeBuffer Allocate(uint size);
    string GetNativeStringLength(string input);
}

public class NativeMemoryService : INativeMemoryService
{
    public NativeBuffer Allocate(uint size)
    {
        return new NativeBuffer(size);
    }

    public string GetNativeStringLength(string input)
    {
        var (success, length) = StdLibBridge.GetStringLength(input);
        return success ? $"Native Length: {length}" : "Error calculating length";
    }
}
