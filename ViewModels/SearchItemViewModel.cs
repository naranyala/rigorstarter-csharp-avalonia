using CommunityToolkit.Mvvm.ComponentModel;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Models;
using RigorStarter.Shared.Utilities;

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

    [ObservableProperty]
    private string _sourceCode = string.Empty;

    [ObservableProperty]
    private UtilityResult? _executionResult;

    [ObservableProperty]
    private ComponentCategory _category = ComponentCategory.InDevelopment;

    [ObservableProperty]
    private string _viewName = string.Empty;

    public Action<SearchItemViewModel>? ExecuteAction { get; set; }

    public string ResultText => ExecutionResult?.Message ?? string.Empty;
    public bool ResultIsSuccess => ExecutionResult?.IsSuccess ?? true;
}
