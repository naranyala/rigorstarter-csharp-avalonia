using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class Card : UserControl, IComponentModule
{
    public new string Name => "Card";
    public string Description => "A container for grouped information";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(Card);
    public bool IsMockup => false;

    public Card()
    {
        InitializeComponent();
    }
}
