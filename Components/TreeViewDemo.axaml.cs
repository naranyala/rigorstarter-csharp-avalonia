using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class TreeViewDemo : UserControl, IComponentModule
{
    public new string Name => "TreeViewDemo";
    public string Description => "A collapsible tree view demo with nested nodes";
    public ComponentCategory Category => ComponentCategory.Pinned;
    public Type ViewType => typeof(TreeViewDemo);
    public bool IsMockup => false;

    public TreeViewDemo()
    {
        InitializeComponent();
    }
}
