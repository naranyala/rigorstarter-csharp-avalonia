using System.Threading.Tasks;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using Xunit;

namespace RigorStarter.Tests;

public class DialogServiceTests
{
    private readonly DialogService _service;

    public DialogServiceTests()
    {
        _service = new DialogService();
    }

    [Fact]
    public async Task ShowOpenFileDialog_ShouldNotThrow()
    {
        // NativeDialogBridge uses zenity which may not be available
        var exception = await Record.ExceptionAsync(() =>
            _service.ShowOpenFileDialogAsync("Test", "*.*")
        );
        Assert.Null(exception);
    }

    [Fact]
    public async Task ShowSaveFileDialog_ShouldNotThrow()
    {
        var exception = await Record.ExceptionAsync(() =>
            _service.ShowSaveFileDialogAsync("Test", "*.*")
        );
        Assert.Null(exception);
    }

    [Fact]
    public async Task ShowMessage_ShouldNotThrow()
    {
        var exception = await Record.ExceptionAsync(() =>
            _service.ShowMessageAsync("Title", "Message")
        );
        Assert.Null(exception);
    }

    [Fact]
    public async Task ShowConfirm_ShouldNotThrow()
    {
        var exception = await Record.ExceptionAsync(() =>
            _service.ShowConfirmDialogAsync("Title", "Message")
        );
        Assert.Null(exception);
    }
}
