using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class TableData : UserControl, IComponentModule
{
    public new string Name => "Table Data";
    public string Description => "A sortable data table demo";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(TableData);
    public bool IsMockup => false;

    public TableData()
    {
        InitializeComponent();
    }
}
