using System;

namespace RigorStarter.Core.Interfaces;

public interface ITrayService
{
    void Initialize(string iconPath, string tooltip);
    void UpdateTooltip(string text);
    void SetVisibility(bool visible);
    void ShowNotification(string title, string message);
    event Action<string> OnMenuItemClicked;
}
