using Avalonia.Controls;
using RigorStarter.Core;
using RigorStarter.ViewModels;

namespace RigorStarter.Components;

public partial class TodoListJson : UserControl
{
    public TodoListJson()
    {
        InitializeComponent();
        ContentRoot.DataContext = ServiceProvider.GetService<TodoListJsonViewModel>();
    }
}
