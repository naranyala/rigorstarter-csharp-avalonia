using CommunityToolkit.Mvvm.ComponentModel;

namespace RigorStarter.ViewModels;

public partial class SearchItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _isComponent;

    [ObservableProperty]
    private bool _isUtility;

    [ObservableProperty]
    private bool _isMockup;

    // For Utilities: The actual code or output
    [ObservableProperty]
    private string _content = string.Empty;
}
