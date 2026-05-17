using CommunityToolkit.Mvvm.ComponentModel;

namespace RigorStarter.ViewModels;

public partial class ComponentDemoViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _isMockup;
}
