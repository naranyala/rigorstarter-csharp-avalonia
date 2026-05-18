using System.Collections.Generic;
using Moq;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Models;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class TodoListViewModelTests
{
    private readonly Mock<ITodoService> _mockService;
    private readonly List<TodoItem> _items;

    public TodoListViewModelTests()
    {
        _items = new List<TodoItem>
        {
            new()
            {
                Id = 1,
                Title = "Task 1",
                IsCompleted = false,
            },
            new()
            {
                Id = 2,
                Title = "Task 2",
                IsCompleted = true,
            },
            new()
            {
                Id = 3,
                Title = "Task 3",
                IsCompleted = false,
            },
        };

        _mockService = new Mock<ITodoService>();
        _mockService.Setup(s => s.GetAll()).Returns(_items);
    }

    [Fact]
    public void Constructor_ShouldLoadItemsFromService()
    {
        var vm = new TodoListViewModel(_mockService.Object);

        Assert.Equal(3, vm.Items.Count);
        Assert.Contains(vm.Items, i => i.Title == "Task 1");
    }

    [Fact]
    public void Constructor_ShouldInitializeService()
    {
        _mockService.Setup(s => s.GetAll()).Returns(new List<TodoItem>());
        var vm = new TodoListViewModel(_mockService.Object);

        _mockService.Verify(s => s.Initialize(), Times.Once);
    }

    [Fact]
    public void RemainingCount_ShouldReturnUncompletedCount()
    {
        var vm = new TodoListViewModel(_mockService.Object);

        Assert.Equal(2, vm.RemainingCount);
    }

    [Fact]
    public void AddItem_ShouldInsertNewItem()
    {
        var vm = new TodoListViewModel(_mockService.Object);
        vm.NewItemTitle = "New task";

        vm.AddItemCommand.Execute(null);

        Assert.Equal(4, vm.Items.Count);
        Assert.Equal("New task", vm.Items[0].Title);
        Assert.Equal(string.Empty, vm.NewItemTitle);
    }

    [Fact]
    public void AddItem_ShouldNotAdd_WhenTitleIsEmpty()
    {
        var vm = new TodoListViewModel(_mockService.Object);
        vm.NewItemTitle = "   ";

        vm.AddItemCommand.Execute(null);

        Assert.Equal(3, vm.Items.Count);
    }

    [Fact]
    public void AddItem_ShouldNotAdd_WhenTitleIsNull()
    {
        var vm = new TodoListViewModel(_mockService.Object);
        vm.NewItemTitle = null!;

        vm.AddItemCommand.Execute(null);

        Assert.Equal(3, vm.Items.Count);
    }

    [Fact]
    public void AddItem_ShouldCallServiceAdd()
    {
        TodoItem? addedItem = null;
        _mockService.Setup(s => s.Add(It.IsAny<TodoItem>())).Callback<TodoItem>(i => addedItem = i);

        var vm = new TodoListViewModel(_mockService.Object);
        vm.NewItemTitle = "Test";

        vm.AddItemCommand.Execute(null);

        _mockService.Verify(s => s.Add(It.IsAny<TodoItem>()), Times.Once);
        Assert.NotNull(addedItem);
        Assert.Equal("Test", addedItem!.Title);
    }

    [Fact]
    public void ToggleItem_ShouldFlipIsCompleted()
    {
        var vm = new TodoListViewModel(_mockService.Object);
        var item = vm.Items[0];
        Assert.False(item.IsCompleted);

        vm.ToggleItemCommand.Execute(item);

        Assert.True(item.IsCompleted);
        _mockService.Verify(s => s.Update(item), Times.Once);
    }

    [Fact]
    public void ToggleItem_ShouldNotThrow_WhenItemIsNull()
    {
        var vm = new TodoListViewModel(_mockService.Object);

        vm.ToggleItemCommand.Execute(null);
    }

    [Fact]
    public void ToggleItem_ShouldUpdateRemainingCount()
    {
        var vm = new TodoListViewModel(_mockService.Object);
        Assert.Equal(2, vm.RemainingCount);

        vm.ToggleItemCommand.Execute(vm.Items[2]); // toggle Task 3 to completed
        Assert.Equal(1, vm.RemainingCount);

        vm.ToggleItemCommand.Execute(vm.Items[2]); // toggle back
        Assert.Equal(2, vm.RemainingCount);
    }

    [Fact]
    public void DeleteItem_ShouldRemoveFromCollection()
    {
        var vm = new TodoListViewModel(_mockService.Object);
        var item = vm.Items[0];

        vm.DeleteItemCommand.Execute(item);

        Assert.Equal(2, vm.Items.Count);
        Assert.DoesNotContain(item, vm.Items);
        _mockService.Verify(s => s.Delete(item.Id), Times.Once);
    }

    [Fact]
    public void DeleteItem_ShouldNotThrow_WhenItemIsNull()
    {
        var vm = new TodoListViewModel(_mockService.Object);

        vm.DeleteItemCommand.Execute(null);
    }

    [Fact]
    public void DeleteItem_ShouldUpdateRemainingCount()
    {
        var vm = new TodoListViewModel(_mockService.Object);
        Assert.Equal(2, vm.RemainingCount);

        vm.DeleteItemCommand.Execute(vm.Items[0]); // delete uncompleted task
        Assert.Equal(1, vm.RemainingCount);
    }
}
