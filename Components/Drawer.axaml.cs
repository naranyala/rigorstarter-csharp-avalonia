using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace RigorStarter.Components;

public partial class Drawer : UserControl
{
    public Drawer()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void ToggleBtn_Click(object sender, RoutedEventArgs e)
    {
        var panel = this.FindControl<Border>("DrawerPanel");
        if (panel != null)
        {
            panel.IsVisible = !panel.IsVisible;
        }
    }
}
