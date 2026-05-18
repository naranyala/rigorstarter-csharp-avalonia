using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RigorStarter.Core.Services;
using RigorStarter.Shared.Models;

namespace RigorStarter.ViewModels;

public partial class TodoListJsonViewModel : SearchItemViewModel
{
    private readonly TodoServiceJson _todoService;

    [ObservableProperty]
    private ObservableCollection<TodoItem> _items = new();

    [ObservableProperty]
    private string _newItemTitle = string.Empty;

    public int RemainingCount => Items.Count(x => !x.IsCompleted);

    public TodoListJsonViewModel(TodoServiceJson todoService)
    {
        _todoService = todoService;
        _todoService.Initialize();
        LoadItems();
    }

    private void LoadItems()
    {
        Items = new ObservableCollection<TodoItem>(_todoService.GetAll());
    }

    [RelayCommand]
    private void AddItem()
    {
        if (string.IsNullOrWhiteSpace(NewItemTitle))
            return;

        var item = new TodoItem { Title = NewItemTitle.Trim() };
        _todoService.Add(item);
        Items.Insert(0, item);
        NewItemTitle = string.Empty;
        OnPropertyChanged(nameof(RemainingCount));
    }

    [RelayCommand]
    private void ToggleItem(TodoItem? item)
    {
        if (item == null)
            return;
        item.IsCompleted = !item.IsCompleted;
        _todoService.Update(item);
        OnPropertyChanged(nameof(RemainingCount));
    }

    [RelayCommand]
    private void DeleteItem(TodoItem? item)
    {
        if (item == null)
            return;
        _todoService.Delete(item.Id);
        Items.Remove(item);
        OnPropertyChanged(nameof(RemainingCount));
    }
}
