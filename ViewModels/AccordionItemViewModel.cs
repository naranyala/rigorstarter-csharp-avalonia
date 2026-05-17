using CommunityToolkit.Mvvm.ComponentModel;

namespace RigorStarter.ViewModels;

public partial class AccordionItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string _header = string.Empty;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _isExpanded;
}
