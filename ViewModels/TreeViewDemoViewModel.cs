using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RigorStarter.Shared.Models;

namespace RigorStarter.ViewModels;

public partial class TreeViewDemoViewModel : SearchItemViewModel
{
    [ObservableProperty]
    private ObservableCollection<TreeNodeItem> _items = new();

    public TreeViewDemoViewModel()
    {
        PopulateDemoData();
    }

    private void PopulateDemoData()
    {
        var root = new TreeNodeItem
        {
            Name = "Project Root",
            IsExpanded = true,
            Children =
            {
                new TreeNodeItem
                {
                    Name = "src",
                    IsExpanded = true,
                    Children =
                    {
                        new TreeNodeItem { Name = "main.cs", IsExpanded = true },
                        new TreeNodeItem { Name = "utils.cs", IsExpanded = true },
                        new TreeNodeItem
                        {
                            Name = "Components",
                            IsExpanded = true,
                            Children =
                            {
                                new TreeNodeItem { Name = "Button.axaml", IsExpanded = true },
                                new TreeNodeItem { Name = "Button.axaml.cs", IsExpanded = true },
                                new TreeNodeItem { Name = "Card.axaml", IsExpanded = true },
                                new TreeNodeItem { Name = "Card.axaml.cs", IsExpanded = true },
                            },
                        },
                        new TreeNodeItem
                        {
                            Name = "ViewModels",
                            IsExpanded = true,
                            Children =
                            {
                                new TreeNodeItem
                                {
                                    Name = "MainWindowViewModel.cs",
                                    IsExpanded = true,
                                },
                            },
                        },
                    },
                },
                new TreeNodeItem
                {
                    Name = "tests",
                    IsExpanded = true,
                    Children =
                    {
                        new TreeNodeItem { Name = "UnitTest1.cs", IsExpanded = true },
                        new TreeNodeItem { Name = "UnitTest2.cs", IsExpanded = true },
                    },
                },
                new TreeNodeItem
                {
                    Name = "docs",
                    IsExpanded = true,
                    Children =
                    {
                        new TreeNodeItem { Name = "architecture.md", IsExpanded = true },
                        new TreeNodeItem { Name = "api.md", IsExpanded = true },
                    },
                },
            },
        };

        Items.Add(root);
    }
}
