namespace RigorStarter.Core.Interfaces;

public interface IThemeService
{
    bool IsDarkTheme { get; set; }
    void ApplyTheme();
}
