using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class TodoList : UserControl, IComponentModule
{
    public new string Name => "TodoList";
    public string Description => "A SQLite-backed todo list demo";
    public ComponentCategory Category => ComponentCategory.Pinned;
    public Type ViewType => typeof(TodoList);
    public bool IsMockup => false;

    public TodoList()
    {
        InitializeComponent();
    }
}
