using System.Threading.Tasks;

namespace RigorStarter.Core.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(
        string title,
        string message,
        NotificationPriority priority = NotificationPriority.Normal
    );
}

public enum NotificationPriority
{
    Low,
    Normal,
    Critical,
}
