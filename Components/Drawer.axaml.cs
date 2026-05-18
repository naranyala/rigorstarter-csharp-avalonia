using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class Drawer : UserControl, IComponentModule
{
    public new string Name => "Drawer";
    public string Description => "A sliding-up panel component";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(Drawer);
    public bool IsMockup => false;

    public Drawer()
    {
        InitializeComponent();
    }

    private void ToggleBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DrawerPanel != null)
        {
            DrawerPanel.IsVisible = !DrawerPanel.IsVisible;
        }
    }
}
