using System;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using Xunit;

namespace RigorStarter.Tests;

public class TrayServiceTests
{
    private readonly TrayService _service;

    public TrayServiceTests()
    {
        _service = new TrayService();
    }

    [Fact]
    public void Initialize_ShouldNotThrow()
    {
        // TrayNativeBridge uses libappindicator P/Invoke which may not be available
        // The initialization should be graceful even if it fails
        var exception = Record.Exception(() => _service.Initialize("test-icon", "test-tooltip"));
        Assert.Null(exception);
    }

    [Fact]
    public void UpdateTooltip_ShouldNotThrow()
    {
        var exception = Record.Exception(() => _service.UpdateTooltip("new tooltip"));
        Assert.Null(exception);
    }

    [Fact]
    public void SetVisibility_ShouldNotThrow()
    {
        var exception = Record.Exception(() => _service.SetVisibility(true));
        Assert.Null(exception);

        exception = Record.Exception(() => _service.SetVisibility(false));
        Assert.Null(exception);
    }

    [Fact]
    public void ShowNotification_ShouldNotThrow()
    {
        var exception = Record.Exception(() => _service.ShowNotification("Title", "Message"));
        Assert.Null(exception);
    }

    [Fact]
    public void OnMenuItemClicked_ShouldBeSettable()
    {
        var exception = Record.Exception(() =>
        {
            _service.OnMenuItemClicked += (item) => { };
            _service.OnMenuItemClicked -= (item) => { };
        });
        Assert.Null(exception);
    }

    [Fact]
    public void TriggerMenuItem_ShouldRaiseEvent()
    {
        string? receivedItem = null;
        _service.OnMenuItemClicked += (item) => receivedItem = item;

        _service.TriggerMenuItem("Quit");

        Assert.Equal("Quit", receivedItem);
    }

    [Fact]
    public void TriggerMenuItem_WithNoSubscribers_ShouldNotThrow()
    {
        var exception = Record.Exception(() => _service.TriggerMenuItem("test"));
        Assert.Null(exception);
    }

    [Fact]
    public void MultipleEventSubscribers_ShouldAllBeNotified()
    {
        int callCount = 0;
        Action<string> handler1 = (item) => callCount++;
        Action<string> handler2 = (item) => callCount++;

        _service.OnMenuItemClicked += handler1;
        _service.OnMenuItemClicked += handler2;

        _service.TriggerMenuItem("test");

        Assert.Equal(2, callCount);
    }
}
