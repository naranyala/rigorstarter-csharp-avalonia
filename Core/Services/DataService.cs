using System;
using System.Collections.ObjectModel;
using System.IO;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Models;
using RigorStarter.Shared.Utilities;
using RigorStarter.ViewModels;

namespace RigorStarter.Core.Services;

public class DataService : IDataService
{
    private readonly ISystemService _systemService;

    public DataService(ISystemService systemService)
    {
        _systemService = systemService;
    }

    public void InitializeComponents(
        ObservableCollection<SearchItemViewModel> searchItems,
        ObservableCollection<SearchItemViewModel> pinnedItems,
        ObservableCollection<SearchItemViewModel> inDevelopmentItems,
        ObservableCollection<SearchItemViewModel> archivesItems,
        ObservableCollection<AccordionItemViewModel> accordionItems
    )
    {
        // Components
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "Accordion",
            "A collapsible section component",
            ComponentCategory.Pinned,
            "Components/Accordion.axaml",
            "Components/Accordion.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "Drawer",
            "A sliding-up panel component",
            ComponentCategory.InDevelopment,
            "Components/Drawer.axaml",
            "Components/Drawer.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "Card",
            "A container for grouped information",
            ComponentCategory.InDevelopment,
            "Components/Card.axaml",
            "Components/Card.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "Button",
            "An interactive clickable element",
            ComponentCategory.InDevelopment,
            "Components/Button.axaml",
            "Components/Button.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "Tabs",
            "A multi-view content switcher",
            ComponentCategory.InDevelopment,
            "Components/Tabs.axaml",
            "Components/Tabs.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "StatusBadge",
            "A small indicator for status states",
            ComponentCategory.InDevelopment,
            "Components/StatusBadge.axaml",
            "Components/StatusBadge.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "MetricCard",
            "A card displaying key performance indicators",
            ComponentCategory.InDevelopment,
            "Components/MetricCard.axaml",
            "Components/MetricCard.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "DataGrid",
            "A powerful table for data display",
            ComponentCategory.InDevelopment,
            null,
            null,
            true
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "ColorPicker",
            "An interactive color selection tool",
            ComponentCategory.InDevelopment,
            null,
            null,
            true
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "CustomChart",
            "Visual representation of data trends",
            ComponentCategory.Archives,
            null,
            null,
            true
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "ToastNotification",
            "Non-intrusive feedback messages",
            ComponentCategory.Archives,
            null,
            null,
            true
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "TodoList",
            "A SQLite-backed todo list demo",
            ComponentCategory.Pinned,
            "Components/TodoList.axaml",
            "Components/TodoList.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "TodoListJson",
            "A JSON-backed todo list demo",
            ComponentCategory.Pinned,
            "Components/TodoListJson.axaml",
            "Components/TodoListJson.axaml.cs"
        );
        AddComponent(
            searchItems,
            pinnedItems,
            inDevelopmentItems,
            archivesItems,
            "TreeViewDemo",
            "A collapsible tree view demo with nested nodes",
            ComponentCategory.Pinned,
            "Components/TreeViewDemo.axaml",
            "Components/TreeViewDemo.axaml.cs"
        );

        // Utilities - Now using the injected ISystemService instead of static calls
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

        // Accordion Items
        accordionItems.Add(
            new AccordionItemViewModel
            {
                Header = "Section 1",
                Content =
                    "This is the content for section 1. It can be any text or even other controls!",
                IsExpanded = true,
            }
        );
        accordionItems.Add(
            new AccordionItemViewModel
            {
                Header = "Section 2",
                Content = "Here is some more detailed information in section 2.",
            }
        );
        accordionItems.Add(
            new AccordionItemViewModel
            {
                Header = "Section 3",
                Content = "Finally, section 3 provides additional context and details.",
            }
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
            new SearchItemViewModel
            {
                Name = name,
                Description = description,
                IsUtility = true,
                SourceCode =
                    $"public static string Get{name.Replace(" ", "")}()\n{{\n    // Implementation in Services/SystemService.cs\n}}",
                ExecuteAction = (item) => item.ExecutionResult = new UtilityResult(true, action()),
            }
        );
    }

    private void AddComponent(
        ObservableCollection<SearchItemViewModel> searchItems,
        ObservableCollection<SearchItemViewModel> pinnedItems,
        ObservableCollection<SearchItemViewModel> inDevelopmentItems,
        ObservableCollection<SearchItemViewModel> archivesItems,
        string name,
        string description,
        ComponentCategory category,
        string? file1 = null,
        string? file2 = null,
        bool isMockup = false
    )
    {
        int lines = 0;
        if (file1 != null)
            lines += CountLines(file1);
        if (file2 != null)
            lines += CountLines(file2);

        var item = new SearchItemViewModel
        {
            Name = name,
            Description = description,
            IsComponent = true,
            IsMockup = isMockup,
            Category = category,
            LinesOfCode = lines,
        };

        searchItems.Add(item);
        switch (category)
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

    private int CountLines(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
                return File.ReadAllLines(filePath).Length;
        }
        catch { }
        return 0;
    }
}
