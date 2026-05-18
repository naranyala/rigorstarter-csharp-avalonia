using System;
using System.Threading.Tasks;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Native;

namespace RigorStarter.Core.Services;

public class NotificationService : INotificationService
{
    public async Task SendNotificationAsync(
        string title,
        string message,
        NotificationPriority priority = NotificationPriority.Normal
    )
    {
        string urgency = priority switch
        {
            NotificationPriority.Low => "low",
            NotificationPriority.Normal => "normal",
            NotificationPriority.Critical => "critical",
            _ => "normal",
        };

        bool success = await NativeNotificationBridge.SendNotificationAsync(
            title,
            message,
            urgency
        );

        if (!success)
        {
            // Log failure via system logger if available
            // SystemLogger.Error($"Failed to send native notification: {title}");
        }
    }
}
