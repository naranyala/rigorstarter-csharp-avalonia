using System.Threading.Tasks;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using Xunit;

namespace RigorStarter.Tests;

public class NotificationTests
{
    [Fact]
    public async Task SendNotification_WithNormalPriority_ShouldCompleteSuccessfully()
    {
        // Arrange
        var service = new NotificationService();

        // Act
        await service.SendNotificationAsync("Test Title", "Test Message");

        // Assert
        // The notify-send command completes without throwing.
        // In a headless CI environment, notify-send may not be available,
        // but the process bridge handles this gracefully.
    }

    [Fact]
    public async Task SendNotification_WithLowPriority_ShouldCompleteSuccessfully()
    {
        // Arrange
        var service = new NotificationService();

        // Act
        await service.SendNotificationAsync("Low", "Low priority test", NotificationPriority.Low);

        // Assert
        // Completes without throwing
    }

    [Fact]
    public async Task SendNotification_WithCriticalPriority_ShouldCompleteSuccessfully()
    {
        // Arrange
        var service = new NotificationService();

        // Act
        await service.SendNotificationAsync(
            "Critical",
            "Critical alert test",
            NotificationPriority.Critical
        );

        // Assert
        // Completes without throwing
    }

    [Fact]
    public async Task SendNotification_WithEmptyTitle_ShouldComplete()
    {
        // Arrange
        var service = new NotificationService();

        // Act
        await service.SendNotificationAsync(string.Empty, "Message body");

        // Assert
        // Completes without throwing
    }

    [Fact]
    public async Task SendNotification_WithEmptyMessage_ShouldComplete()
    {
        // Arrange
        var service = new NotificationService();

        // Act
        await service.SendNotificationAsync("Title", string.Empty);

        // Assert
        // Completes without throwing
    }

    [Fact]
    public async Task SendNotification_WithSpecialCharacters_ShouldComplete()
    {
        // Arrange
        var service = new NotificationService();

        // Act
        await service.SendNotificationAsync("Test \"Quotes\"", "Message with $pecial @chars!");

        // Assert
        // Completes without throwing
    }
}
