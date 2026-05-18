using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class MetricCard : UserControl, IComponentModule
{
    public new string Name => "MetricCard";
    public string Description => "A card displaying key performance indicators";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(MetricCard);
    public bool IsMockup => false;

    public MetricCard()
    {
        InitializeComponent();
    }
}
