using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Utilities;

namespace RigorStarter.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private bool _isSearchPanelOpen;

    [ObservableProperty]
    private bool _isDarkTheme;

    [ObservableProperty]
    private string _searchText = string.Empty;

    public ObservableCollection<SearchItemViewModel> SearchItems { get; } = new();

    [ObservableProperty]
    private ObservableCollection<SearchItemViewModel> _filteredItems = new();

    [ObservableProperty]
    private SearchItemViewModel? _selectedItem;

    public bool IsAccordionSelected => SelectedItem?.Name == "Accordion";
    public bool IsDrawerSelected => SelectedItem?.Name == "Drawer";
    public bool IsCardSelected => SelectedItem?.Name == "Card";
    public bool IsButtonSelected => SelectedItem?.Name == "Button";
    public bool IsTabsSelected => SelectedItem?.Name == "Tabs";
    public bool IsStatusBadgeSelected => SelectedItem?.Name == "StatusBadge";
    public bool IsMetricCardSelected => SelectedItem?.Name == "MetricCard";
    public bool IsUtilitySelected => SelectedItem?.IsUtility ?? false;
    public bool IsMockupSelected => SelectedItem?.IsMockup ?? false;
    public bool IsAnyItemSelected => SelectedItem != null;

    public int TotalItems => SearchItems.Count;

    public ObservableCollection<AccordionItemViewModel> AccordionItems { get; } = new();

    public ObservableCollection<SearchItemViewModel> PinnedItems { get; } = new();
    public ObservableCollection<SearchItemViewModel> InDevelopmentItems { get; } = new();
    public ObservableCollection<SearchItemViewModel> ArchivesItems { get; } = new();

    public MainWindowViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _dataService.InitializeComponents(
            SearchItems,
            PinnedItems,
            InDevelopmentItems,
            ArchivesItems,
            AccordionItems
        );

        FilteredItems = new ObservableCollection<SearchItemViewModel>(SearchItems);
    }

    [RelayCommand]
    private void ToggleSearch()
    {
        IsSearchPanelOpen = !IsSearchPanelOpen;
        if (!IsSearchPanelOpen)
        {
            SearchText = string.Empty;
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        Application.Current!.RequestedThemeVariant = IsDarkTheme
            ? ThemeVariant.Dark
            : ThemeVariant.Light;
    }

    [RelayCommand]
    private void SelectItem(SearchItemViewModel item)
    {
        if (item.IsUtility)
        {
            item.ExecuteAction?.Invoke(item);
        }

        SelectedItem = item;
        IsSearchPanelOpen = false;
        OnPropertyChanged(nameof(IsAccordionSelected));
        OnPropertyChanged(nameof(IsDrawerSelected));
        OnPropertyChanged(nameof(IsCardSelected));
        OnPropertyChanged(nameof(IsButtonSelected));
        OnPropertyChanged(nameof(IsTabsSelected));
        OnPropertyChanged(nameof(IsStatusBadgeSelected));
        OnPropertyChanged(nameof(IsMetricCardSelected));
        OnPropertyChanged(nameof(IsUtilitySelected));
        OnPropertyChanged(nameof(IsMockupSelected));
        OnPropertyChanged(nameof(IsAnyItemSelected));
    }

    [RelayCommand]
    private void GoToDashboard()
    {
        SelectedItem = null;
        OnPropertyChanged(nameof(IsAccordionSelected));
        OnPropertyChanged(nameof(IsDrawerSelected));
        OnPropertyChanged(nameof(IsCardSelected));
        OnPropertyChanged(nameof(IsButtonSelected));
        OnPropertyChanged(nameof(IsTabsSelected));
        OnPropertyChanged(nameof(IsStatusBadgeSelected));
        OnPropertyChanged(nameof(IsMetricCardSelected));
        OnPropertyChanged(nameof(IsUtilitySelected));
        OnPropertyChanged(nameof(IsMockupSelected));
        OnPropertyChanged(nameof(IsAnyItemSelected));
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            FilteredItems = new ObservableCollection<SearchItemViewModel>(SearchItems);
        }
        else
        {
            var lowerValue = value.ToLower();
            var filtered = SearchItems
                .Where(x =>
                    x.Name.ToLower().Contains(lowerValue)
                    || x.Description.ToLower().Contains(lowerValue)
                )
                .ToList();
            FilteredItems = new ObservableCollection<SearchItemViewModel>(filtered);
        }
    }
}
