using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class MarkdownDemo : UserControl, IComponentModule
{
    public new string Name => "Markdown Demo";
    public string Description => "A markdown editor and viewer";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(MarkdownDemo);
    public bool IsMockup => false;

    public MarkdownDemo()
    {
        InitializeComponent();
    }
}
