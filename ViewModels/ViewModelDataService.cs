using System;
using System.Collections.ObjectModel;
using System.IO;
using RigorStarter.Utilities;
using RigorStarter.ViewModels;

namespace RigorStarter.ViewModels;

public class ViewModelDataService
{
    public void InitializeComponents(
        ObservableCollection<SearchItemViewModel> searchItems,
        ObservableCollection<SearchItemViewModel> pinnedItems,
        ObservableCollection<SearchItemViewModel> inDevelopmentItems,
        ObservableCollection<SearchItemViewModel> archivesItems,
        ObservableCollection<AccordionItemViewModel> accordionItems)
    {
        // Components
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "Accordion", "A collapsible section component", ComponentCategory.Pinned, "Components/Accordion.axaml", "Components/Accordion.axaml.cs");
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "Drawer", "A sliding-up panel component", ComponentCategory.InDevelopment, "Components/Drawer.axaml", "Components/Drawer.axaml.cs");
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "Card", "A container for grouped information", ComponentCategory.InDevelopment, "Components/Card.axaml", "Components/Card.axaml.cs");
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "Button", "An interactive clickable element", ComponentCategory.InDevelopment, "Components/Button.axaml", "Components/Button.axaml.cs");
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "DataGrid", "A powerful table for data display", ComponentCategory.InDevelopment, null, null, true);
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "ColorPicker", "An interactive color selection tool", ComponentCategory.InDevelopment, null, null, true);
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "CustomChart", "Visual representation of data trends", ComponentCategory.Archives, null, null, true);
        AddComponent(searchItems, pinnedItems, inDevelopmentItems, archivesItems, "ToastNotification", "Non-intrusive feedback messages", ComponentCategory.Archives, null, null, true);
        
        // Utilities
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "Network Utility", 
            Description = "System network information utility", 
            IsUtility = true, 
            SourceCode = "public static string GetNetworkSummary()\n{\n    // Implementation in Utilities/NetworkUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = NetworkUtility.GetNetworkSummary()
        });
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "Disk Utility", 
            Description = "Storage and drive space summary", 
            IsUtility = true, 
            SourceCode = "public static string GetDiskSummary()\n{\n    // Implementation in Utilities/DiskUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = DiskUtility.GetDiskSummary()
        });
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "System Info Utility", 
            Description = "OS and hardware specifications", 
            IsUtility = true, 
            SourceCode = "public static string GetSystemSummary()\n{\n    // Implementation in Utilities/SystemInfoUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = SystemInfoUtility.GetSystemSummary()
        });
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "Process Utility", 
            Description = "Top memory-consuming processes", 
            IsUtility = true, 
            SourceCode = "public static string GetTopProcesses()\n{\n    // Implementation in Utilities/ProcessUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = ProcessUtility.GetTopProcesses()
        });
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "Memory Utility", 
            Description = "RAM and memory usage summary", 
            IsUtility = true, 
            SourceCode = "public static UtilityResult GetMemorySummary()\n{\n    // Implementation in Utilities/MemoryUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = MemoryUtility.GetMemorySummary()
        });
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "CPU Utility", 
            Description = "Processor utilization and info", 
            IsUtility = true, 
            SourceCode = "public static UtilityResult GetCpuSummary()\n{\n    // Implementation in Utilities/CpuUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = CpuUtility.GetCpuSummary()
        });

        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "Disk Utility", 
            Description = "Storage and drive space summary", 
            IsUtility = true, 
            SourceCode = "public static string GetDiskSummary()\n{\n    // Implementation in Utilities/DiskUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = DiskUtility.GetDiskSummary()
        });
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "System Info Utility", 
            Description = "OS and hardware specifications", 
            IsUtility = true, 
            SourceCode = "public static string GetSystemSummary()\n{\n    // Implementation in Utilities/SystemInfoUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = SystemInfoUtility.GetSystemSummary()
        });
        searchItems.Add(new SearchItemViewModel 
        { 
            Name = "Process Utility", 
            Description = "Top memory-consuming processes", 
            IsUtility = true, 
            SourceCode = "public static string GetTopProcesses()\n{\n    // Implementation in Utilities/ProcessUtility.cs\n}",
            ExecuteAction = (item) => item.ExecutionResult = ProcessUtility.GetTopProcesses()
        });

        // Accordion Items
        accordionItems.Add(new AccordionItemViewModel { Header = "Section 1", Content = "This is the content for section 1. It can be any text or even other controls!", IsExpanded = true });
        accordionItems.Add(new AccordionItemViewModel { Header = "Section 2", Content = "Here is some more detailed information in section 2." });
        accordionItems.Add(new AccordionItemViewModel { Header = "Section 3", Content = "Finally, section 3 provides additional context and details." });
    }

    private void AddComponent(
        ObservableCollection<SearchItemViewModel> searchItems,
        ObservableCollection<SearchItemViewModel> pinnedItems,
        ObservableCollection<SearchItemViewModel> inDevelopmentItems,
        ObservableCollection<SearchItemViewModel> archivesItems,
        string name, string description, ComponentCategory category, string? file1 = null, string? file2 = null, bool isMockup = false)
    {
        int lines = 0;
        if (file1 != null) lines += CountLines(file1);
        if (file2 != null) lines += CountLines(file2);

        var item = new SearchItemViewModel
        {
            Name = name,
            Description = description,
            IsComponent = true,
            IsMockup = isMockup,
            Category = category,
            LinesOfCode = lines
        };

        searchItems.Add(item);

        switch (category)
        {
            case ComponentCategory.Pinned: pinnedItems.Add(item); break;
            case ComponentCategory.InDevelopment: inDevelopmentItems.Add(item); break;
            case ComponentCategory.Archives: archivesItems.Add(item); break;
        }
    }

    private int CountLines(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllLines(filePath).Length;
            }
        }
        catch { }
        return 0;
    }
}
