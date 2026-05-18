using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;

namespace RigorStarter.Components;

public partial class TodoListJson : UserControl, IComponentModule
{
    public new string Name => "TodoListJson";
    public string Description => "A JSON-backed todo list demo";
    public ComponentCategory Category => ComponentCategory.Pinned;
    public Type ViewType => typeof(TodoListJson);
    public bool IsMockup => false;

    public TodoListJson()
    {
        InitializeComponent();
    }
}
