using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class Tabs : UserControl, IComponentModule
{
    public new string Name => "Tabs";
    public string Description => "A multi-view content switcher";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(Tabs);
    public bool IsMockup => false;

    public Tabs()
    {
        InitializeComponent();
    }
}
