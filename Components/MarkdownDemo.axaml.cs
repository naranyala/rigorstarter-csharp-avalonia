using Avalonia.Controls;
using RigorStarter.Core;
using RigorStarter.ViewModels;

namespace RigorStarter.Components;

public partial class MarkdownDemo : UserControl
{
    public MarkdownDemo()
    {
        InitializeComponent();
        ContentRoot.DataContext = ServiceProvider.GetService<MarkdownDemoViewModel>();
    }
}
