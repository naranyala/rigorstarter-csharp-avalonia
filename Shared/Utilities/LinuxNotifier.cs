using System.Threading.Tasks;

namespace RigorStarter.Shared.Utilities;

public static class LinuxNotifier
{
    public static async Task SendNotification(
        string title,
        string message,
        string urgency = "normal"
    )
    {
        // uses notify-send which is standard on most Linux desktops (libnotify)
        await LinuxShell.ExecuteAsync("notify-send", $"-u {urgency} \"{title}\" \"{message}\"");
    }
}
