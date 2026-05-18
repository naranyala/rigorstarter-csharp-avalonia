using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class Accordion : UserControl, IComponentModule
{
    public new string Name => "Accordion";
    public string Description => "A collapsible section component";
    public ComponentCategory Category => ComponentCategory.Pinned;
    public Type ViewType => typeof(Accordion);
    public bool IsMockup => false;

    public Accordion()
    {
        InitializeComponent();
    }
}
