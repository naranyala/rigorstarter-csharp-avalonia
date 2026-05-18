using System.Threading.Tasks;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Native;

namespace RigorStarter.Core.Services;

public class DialogService : IDialogService
{
    public async Task<string?> ShowOpenFileDialogAsync(string title, string filter)
    {
        return await NativeDialogBridge.OpenFileAsync(title, filter);
    }

    public async Task<string?> ShowSaveFileDialogAsync(string title, string filter)
    {
        return await NativeDialogBridge.SaveFileAsync(title, filter);
    }

    public async Task ShowMessageAsync(string title, string message)
    {
        await NativeDialogBridge.ShowMessageAsync(title, message);
    }

    public async Task<bool> ShowConfirmDialogAsync(string title, string message)
    {
        return await NativeDialogBridge.ConfirmAsync(title, message);
    }
}
