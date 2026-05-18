using System.Threading.Tasks;

namespace RigorStarter.Core.Interfaces;

public interface IDialogService
{
    Task<string?> ShowOpenFileDialogAsync(string title, string filter);
    Task<string?> ShowSaveFileDialogAsync(string title, string filter);
    Task ShowMessageAsync(string title, string message);
    Task<bool> ShowConfirmDialogAsync(string title, string message);
}
