using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RigorStarter.Shared.Models;

namespace RigorStarter.ViewModels;

public enum SortDirection
{
    None,
    Ascending,
    Descending,
}

public partial class TableDataViewModel : SearchItemViewModel
{
    [ObservableProperty]
    private ObservableCollection<TableDataRow> _items = new();

    [ObservableProperty]
    private string _currentSortColumn = string.Empty;

    [ObservableProperty]
    private SortDirection _currentSortDirection = SortDirection.None;

    public TableDataViewModel()
    {
        LoadSampleData();
    }

    private void LoadSampleData()
    {
        var data = new List<TableDataRow>
        {
            new()
            {
                Name = "Alpha",
                Value = 100,
                Status = "Active",
                Category = "A",
            },
            new()
            {
                Name = "Beta",
                Value = 50,
                Status = "Inactive",
                Category = "B",
            },
            new()
            {
                Name = "Gamma",
                Value = 150,
                Status = "Active",
                Category = "A",
            },
            new()
            {
                Name = "Delta",
                Value = 200,
                Status = "Pending",
                Category = "C",
            },
            new()
            {
                Name = "Epsilon",
                Value = 75,
                Status = "Active",
                Category = "B",
            },
            new()
            {
                Name = "Zeta",
                Value = 125,
                Status = "Inactive",
                Category = "C",
            },
            new()
            {
                Name = "Eta",
                Value = 25,
                Status = "Pending",
                Category = "A",
            },
        };

        Items = new ObservableCollection<TableDataRow>(data);
    }

    [RelayCommand]
    public void Sort(string columnName)
    {
        if (CurrentSortColumn == columnName)
        {
            if (CurrentSortDirection == SortDirection.Ascending)
            {
                CurrentSortDirection = SortDirection.Descending;
            }
            else
            {
                CurrentSortDirection = SortDirection.None;
            }
        }
        else
        {
            CurrentSortColumn = columnName;
            CurrentSortDirection = SortDirection.Ascending;
        }

        ApplySort();
    }

    private void ApplySort()
    {
        if (CurrentSortDirection == SortDirection.None)
        {
            LoadSampleData();
            return;
        }

        IEnumerable<TableDataRow> sorted;
        switch (CurrentSortColumn)
        {
            case "Name":
                sorted =
                    CurrentSortDirection == SortDirection.Ascending
                        ? Items.OrderBy(x => x.Name)
                        : Items.OrderByDescending(x => x.Name);
                break;
            case "Value":
                sorted =
                    CurrentSortDirection == SortDirection.Ascending
                        ? Items.OrderBy(x => x.Value)
                        : Items.OrderByDescending(x => x.Value);
                break;
            case "Status":
                sorted =
                    CurrentSortDirection == SortDirection.Ascending
                        ? Items.OrderBy(x => x.Status)
                        : Items.OrderByDescending(x => x.Status);
                break;
            case "Category":
                sorted =
                    CurrentSortDirection == SortDirection.Ascending
                        ? Items.OrderBy(x => x.Category)
                        : Items.OrderByDescending(x => x.Category);
                break;
            default:
                return;
        }

        Items = new ObservableCollection<TableDataRow>(sorted);
    }

    public string GetSortIcon(string columnName)
    {
        if (CurrentSortColumn != columnName)
            return string.Empty;

        return CurrentSortDirection switch
        {
            SortDirection.Ascending => " ▲",
            SortDirection.Descending => " ▼",
            _ => string.Empty,
        };
    }
}
