using Avalonia.Controls;
using RigorStarter.Core;
using RigorStarter.ViewModels;

namespace RigorStarter.Components;

public partial class TodoList : UserControl
{
    public TodoList()
    {
        InitializeComponent();
        ContentRoot.DataContext = ServiceProvider.GetService<TodoListViewModel>();
    }
}
