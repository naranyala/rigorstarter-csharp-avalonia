using System;
using System.Collections.ObjectModel;
using System.IO;
using RigorStarter.Core;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Models;
using RigorStarter.Shared.Utilities;
using RigorStarter.ViewModels;

namespace RigorStarter.Core.Services;

public class DataService : IDataService
{
    private readonly ISystemService _systemService;
    private readonly ComponentRegistry _componentRegistry;

    public DataService(ISystemService systemService, ComponentRegistry componentRegistry)
    {
        _systemService = systemService;
        _componentRegistry = componentRegistry;
    }

    public void InitializeComponents(
        ObservableCollection<SearchItemViewModel> searchItems,
        ObservableCollection<SearchItemViewModel> pinnedItems,
        ObservableCollection<SearchItemViewModel> inDevelopmentItems,
        ObservableCollection<SearchItemViewModel> archivesItems,
        ObservableCollection<AccordionItemViewModel> accordionItems
    )
    {
        // 1. Auto-Discover and Add Components from Registry
        foreach (var module in _componentRegistry.Modules)
        {
            Type vmType = module.Name switch
            {
                "Accordion" => typeof(AccordionViewModel),
                "Drawer" => typeof(DrawerViewModel),
                "Card" => typeof(CardViewModel),
                "Button" => typeof(ButtonViewModel),
                "Tabs" => typeof(TabsViewModel),
                "StatusBadge" => typeof(StatusBadgeViewModel),
                "MetricCard" => typeof(MetricCardViewModel),
                "TodoList" => typeof(TodoListViewModel),
                "TodoListJson" => typeof(TodoListJsonViewModel),
                "TreeViewDemo" => typeof(TreeViewDemoViewModel),
                "Table Data" => typeof(TableDataViewModel),
                "Markdown Demo" => typeof(MarkdownDemoViewModel),
                _ => typeof(SearchItemViewModel),
            };

            SearchItemViewModel item;
            try
            {
                item = (SearchItemViewModel)ServiceProvider.GetService(vmType);
            }
            catch
            {
                item = (SearchItemViewModel)Activator.CreateInstance(vmType)!;
            }

            item.Name = module.Name;
            item.Description = module.Description;
            item.Category = module.Category;
            item.IsMockup = module.IsMockup;
            item.IsComponent = true;
            item.ViewName = module.Name.Replace(" ", "");

            searchItems.Add(item);
            switch (module.Category)
            {
                case ComponentCategory.Pinned:
                    pinnedItems.Add(item);
                    break;
                case ComponentCategory.InDevelopment:
                    inDevelopmentItems.Add(item);
                    break;
                case ComponentCategory.Archives:
                    archivesItems.Add(item);
                    break;
            }
        }

        // 2. Add Utilities
        AddUtility(
            searchItems,
            "Network Utility",
            "System network information utility",
            () => _systemService.GetNetworkSummary()
        );
        AddUtility(
            searchItems,
            "Disk Utility",
            "Storage and drive space summary",
            () => _systemService.GetDiskSummary()
        );
        AddUtility(
            searchItems,
            "System Info Utility",
            "OS and hardware specifications",
            () => _systemService.GetSystemSummary()
        );
        AddUtility(
            searchItems,
            "Process Utility",
            "Top memory-consuming processes",
            () => _systemService.GetTopProcesses()
        );
        AddUtility(
            searchItems,
            "Memory Utility",
            "RAM and memory usage summary",
            () => _systemService.GetMemorySummary()
        );
        AddUtility(
            searchItems,
            "CPU Utility",
            "Processor utilization and info",
            () => _systemService.GetCpuSummary()
        );

        // 3. Static Accordion Items
        accordionItems.Add(
            new AccordionItemViewModel
            {
                Header = "Section 1",
                Content = "Content 1",
                IsExpanded = true,
            }
        );
        accordionItems.Add(
            new AccordionItemViewModel { Header = "Section 2", Content = "Content 2" }
        );
        accordionItems.Add(
            new AccordionItemViewModel { Header = "Section 3", Content = "Content 3" }
        );
    }

    private void AddUtility(
        ObservableCollection<SearchItemViewModel> searchItems,
        string name,
        string description,
        Func<string> action
    )
    {
        searchItems.Add(
            new UtilityViewModel
            {
                Name = name,
                Description = description,
                IsUtility = true,
                SourceCode =
                    $"public static string Get{name.Replace(" ", "")}()\n{{\n    // Implementation in Services/SystemService.cs\n}}",
                ViewName = "Utility",
                ExecuteAction = (item) => item.ExecutionResult = new UtilityResult(true, action()),
            }
        );
    }
}
