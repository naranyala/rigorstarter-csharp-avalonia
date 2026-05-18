using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class Button : UserControl, IComponentModule
{
    public new string Name => "Button";
    public string Description => "An interactive clickable element";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(Button);
    public bool IsMockup => false;

    public Button()
    {
        InitializeComponent();
    }
}
