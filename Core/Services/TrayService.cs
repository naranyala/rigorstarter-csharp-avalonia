using System;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Native;

namespace RigorStarter.Core.Services;

public class TrayService : ITrayService
{
    public event Action<string>? OnMenuItemClicked;

    public void Initialize(string iconPath, string tooltip)
    {
        // In a real app, the iconPath would be a registered system icon name
        bool success = TrayNativeBridge.Initialize("rigorstarter", iconPath, tooltip);
        if (!success)
        {
            // Fallback: Log error via SystemLogger
            // SystemLogger.Error("Failed to initialize native tray indicator");
        }
    }

    public void UpdateTooltip(string text)
    {
        // Native update call would go here
    }

    public void SetVisibility(bool visible)
    {
        TrayNativeBridge.SetVisibility(visible);
    }

    public void ShowNotification(string title, string message)
    {
        // This would typically involve calling libnotify via FFI
        Console.WriteLine($"[TRAY NOTIFICATION] {title}: {message}");
    }

    // Method to simulate a click from the native side
    public void TriggerMenuItem(string item)
    {
        OnMenuItemClicked?.Invoke(item);
    }
}
