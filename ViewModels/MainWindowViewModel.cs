using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RigorStarter.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isSearchPanelOpen;

    [ObservableProperty]
    private string _searchText = string.Empty;

    public ObservableCollection<SearchItemViewModel> SearchItems { get; } = new();

    [ObservableProperty]
    private ObservableCollection<SearchItemViewModel> _filteredItems = new();

    [ObservableProperty]
    private SearchItemViewModel? _selectedItem;

    public bool IsAccordionSelected => SelectedItem?.Name == "Accordion";
    public bool IsDrawerSelected => SelectedItem?.Name == "Drawer";
    public bool IsUtilitySelected => SelectedItem?.IsUtility ?? false;
    public bool IsMockupSelected => SelectedItem?.IsMockup ?? false;
    public bool IsAnyItemSelected => SelectedItem != null;

    public int TotalItems => SearchItems.Count;

    public ObservableCollection<AccordionItemViewModel> AccordionItems { get; } = new();

    public MainWindowViewModel()
    {
        // Components
        SearchItems.Add(new SearchItemViewModel { Name = "Accordion", Description = "A collapsible section component", IsComponent = true, IsMockup = false });
        SearchItems.Add(new SearchItemViewModel { Name = "Drawer", Description = "A sliding-up panel component", IsComponent = true, IsMockup = false });
        SearchItems.Add(new SearchItemViewModel { Name = "DataGrid", Description = "A powerful table for data display", IsComponent = true, IsMockup = true });
        SearchItems.Add(new SearchItemViewModel { Name = "ColorPicker", Description = "An interactive color selection tool", IsComponent = true, IsMockup = true });
        SearchItems.Add(new SearchItemViewModel { Name = "CustomChart", Description = "Visual representation of data trends", IsComponent = true, IsMockup = true });
        SearchItems.Add(new SearchItemViewModel { Name = "ToastNotification", Description = "Non-intrusive feedback messages", IsComponent = true, IsMockup = true });
        
        // Utilities
        SearchItems.Add(new SearchItemViewModel 
        { 
            Name = "Network Utility", 
            Description = "System network information utility", 
            IsUtility = true, 
            Content = "public static class NetworkUtility\n{\n    public static string GetNetworkSummary()\n    {\n        // Implementation in Utilities/NetworkUtility.cs\n    }\n}" 
        });

        AccordionItems.Add(new AccordionItemViewModel { Header = "Section 1", Content = "This is the content for section 1. It can be any text or even other controls!", IsExpanded = true });
        AccordionItems.Add(new AccordionItemViewModel { Header = "Section 2", Content = "Here is some more detailed information in section 2." });
        AccordionItems.Add(new AccordionItemViewModel { Header = "Section 3", Content = "Finally, section 3 provides additional context and details." });
        
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
    private void SelectItem(SearchItemViewModel item)
    {
        SelectedItem = item;
        IsSearchPanelOpen = false;
        OnPropertyChanged(nameof(IsAccordionSelected));
        OnPropertyChanged(nameof(IsDrawerSelected));
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
            var filtered = SearchItems.Where(x => 
                x.Name.ToLower().Contains(lowerValue) || 
                x.Description.ToLower().Contains(lowerValue)).ToList();
            FilteredItems = new ObservableCollection<SearchItemViewModel>(filtered);
        }
    }
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
    private void SelectComponent(ComponentDemoViewModel component)
    {
        SelectedComponent = component;
        IsSearchPanelOpen = false;
        OnPropertyChanged(nameof(IsAccordionSelected));
        OnPropertyChanged(nameof(IsDrawerSelected));
        OnPropertyChanged(nameof(IsMockupSelected));
        OnPropertyChanged(nameof(IsAnyComponentSelected));
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            FilteredComponents = new ObservableCollection<ComponentDemoViewModel>(ComponentDemos);
        }
        else
        {
            var lowerValue = value.ToLower();
            var filtered = ComponentDemos.Where(x => 
                x.Name.ToLower().Contains(lowerValue) || 
                x.Description.ToLower().Contains(lowerValue)).ToList();
            FilteredComponents = new ObservableCollection<ComponentDemoViewModel>(filtered);
        }
    }
}
