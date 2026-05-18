using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RigorStarter.Shared.Models;

public partial class TreeNodeItem : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private ObservableCollection<TreeNodeItem> _children = new();

    [ObservableProperty]
    private bool _isExpanded = true;
}
