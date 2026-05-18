using System;
using System.IO;
using RigorStarter.Core.Services;
using RigorStarter.Shared.Models;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class TodoListJsonViewModelTests : IDisposable
{
    private readonly string _filePath;
    private readonly TodoServiceJson _service;

    public TodoListJsonViewModelTests()
    {
        _filePath = Path.Combine(Path.GetTempPath(), $"todos_json_vm_test_{Guid.NewGuid()}.json");
        _service = new TodoServiceJson(_filePath);
        _service.Initialize();
        _service.Add(new TodoItem { Title = "Task 1", IsCompleted = false });
        _service.Add(new TodoItem { Title = "Task 2", IsCompleted = true });
        _service.Add(new TodoItem { Title = "Task 3", IsCompleted = false });
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    [Fact]
    public void Constructor_ShouldLoadItemsFromService()
    {
        var vm = new TodoListJsonViewModel(_service);

        Assert.Equal(3, vm.Items.Count);
        Assert.Contains(vm.Items, i => i.Title == "Task 1");
    }

    [Fact]
    public void RemainingCount_ShouldReturnUncompletedCount()
    {
        var vm = new TodoListJsonViewModel(_service);
        Assert.Equal(2, vm.RemainingCount);
    }

    [Fact]
    public void AddItem_ShouldInsertNewItem()
    {
        var vm = new TodoListJsonViewModel(_service);
        vm.NewItemTitle = "New task";

        vm.AddItemCommand.Execute(null);

        Assert.Equal(4, vm.Items.Count);
        Assert.Equal("New task", vm.Items[0].Title);
        Assert.Equal(string.Empty, vm.NewItemTitle);
    }

    [Fact]
    public void AddItem_ShouldNotAdd_WhenTitleIsEmpty()
    {
        var vm = new TodoListJsonViewModel(_service);
        vm.NewItemTitle = "   ";

        vm.AddItemCommand.Execute(null);

        Assert.Equal(3, vm.Items.Count);
    }

    [Fact]
    public void AddItem_ShouldPersist()
    {
        var vm = new TodoListJsonViewModel(_service);
        vm.NewItemTitle = "Persist test";
        vm.AddItemCommand.Execute(null);

        var service2 = new TodoServiceJson(_filePath);
        service2.Initialize();
        Assert.Contains(service2.GetAll(), i => i.Title == "Persist test");
    }

    [Fact]
    public void ToggleItem_ShouldFlipIsCompleted()
    {
        var vm = new TodoListJsonViewModel(_service);
        var item = vm.Items.First(i => !i.IsCompleted);

        vm.ToggleItemCommand.Execute(item);

        Assert.True(item.IsCompleted);
    }

    [Fact]
    public void ToggleItem_ShouldNotThrow_WhenItemIsNull()
    {
        var vm = new TodoListJsonViewModel(_service);
        vm.ToggleItemCommand.Execute(null);
    }

    [Fact]
    public void ToggleItem_ShouldUpdateRemainingCount()
    {
        var vm = new TodoListJsonViewModel(_service);
        Assert.Equal(2, vm.RemainingCount);

        var uncompleted = vm.Items.First(i => !i.IsCompleted);
        vm.ToggleItemCommand.Execute(uncompleted);
        Assert.Equal(1, vm.RemainingCount);
    }

    [Fact]
    public void DeleteItem_ShouldRemoveFromCollection()
    {
        var vm = new TodoListJsonViewModel(_service);
        var item = vm.Items[0];

        vm.DeleteItemCommand.Execute(item);

        Assert.Equal(2, vm.Items.Count);
        Assert.DoesNotContain(item, vm.Items);
    }

    [Fact]
    public void DeleteItem_ShouldNotThrow_WhenItemIsNull()
    {
        var vm = new TodoListJsonViewModel(_service);
        vm.DeleteItemCommand.Execute(null);
    }

    [Fact]
    public void DeleteItem_ShouldUpdateRemainingCount()
    {
        var vm = new TodoListJsonViewModel(_service);
        Assert.Equal(2, vm.RemainingCount);

        vm.DeleteItemCommand.Execute(vm.Items.First(i => !i.IsCompleted));
        Assert.Equal(1, vm.RemainingCount);
    }

    [Fact]
    public void DeleteItem_ShouldPersist()
    {
        var vm = new TodoListJsonViewModel(_service);
        vm.DeleteItemCommand.Execute(vm.Items[0]);

        var service2 = new TodoServiceJson(_filePath);
        service2.Initialize();
        Assert.Equal(2, service2.GetAll().Count);
    }
}
