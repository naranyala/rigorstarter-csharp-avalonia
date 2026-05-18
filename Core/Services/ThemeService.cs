using Avalonia;
using Avalonia.Styling;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Core.Services;

public class ThemeService : IThemeService
{
    public bool IsDarkTheme { get; set; }

    public void ApplyTheme()
    {
        if (Application.Current != null)
        {
            Application.Current.RequestedThemeVariant = IsDarkTheme
                ? ThemeVariant.Dark
                : ThemeVariant.Light;
        }
    }
}
