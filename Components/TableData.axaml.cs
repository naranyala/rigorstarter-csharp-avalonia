using Avalonia.Controls;
using RigorStarter.Core;
using RigorStarter.ViewModels;

namespace RigorStarter.Components;

public partial class TableData : UserControl
{
    public TableData()
    {
        InitializeComponent();
        ContentRoot.DataContext = ServiceProvider.GetService<TableDataViewModel>();
    }
}
