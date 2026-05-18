using Avalonia.Controls;
using RigorStarter.Core;
using RigorStarter.ViewModels;

namespace RigorStarter.Components;

public partial class TreeViewDemo : UserControl
{
    public TreeViewDemo()
    {
        InitializeComponent();
        DataContext = ServiceProvider.GetService<TreeViewDemoViewModel>();
    }
}
