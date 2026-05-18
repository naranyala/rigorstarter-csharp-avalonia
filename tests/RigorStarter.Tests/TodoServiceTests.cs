using System;
using System.IO;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.Shared.Models;
using Xunit;

namespace RigorStarter.Tests;

public class TodoServiceTests : IDisposable
{
    private readonly string _dbPath;
    private readonly ITodoService _service;

    public TodoServiceTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"todos_test_{Guid.NewGuid()}.db");
        _service = new TodoService(_dbPath);
        _service.Initialize();
    }

    public void Dispose()
    {
        if (File.Exists(_dbPath))
            File.Delete(_dbPath);
    }

    [Fact]
    public void GetAll_ShouldReturnEmpty_WhenNoItems()
    {
        var items = _service.GetAll();
        Assert.Empty(items);
    }

    [Fact]
    public void Add_ShouldCreateItem_WithGeneratedId()
    {
        var item = new TodoItem { Title = "Test task", IsCompleted = false };
        _service.Add(item);

        Assert.True(item.Id > 0);

        var items = _service.GetAll();
        Assert.Single(items);
        Assert.Equal("Test task", items[0].Title);
        Assert.False(items[0].IsCompleted);
    }

    [Fact]
    public void GetAll_ShouldReturnItemsInDescendingOrder()
    {
        _service.Add(new TodoItem { Title = "First" });
        _service.Add(new TodoItem { Title = "Second" });
        _service.Add(new TodoItem { Title = "Third" });

        var items = _service.GetAll();
        Assert.Equal(3, items.Count);
        Assert.Equal("Third", items[0].Title);
        Assert.Equal("First", items[2].Title);
    }

    [Fact]
    public void Update_ShouldModifyExistingItem()
    {
        var item = new TodoItem { Title = "Original" };
        _service.Add(item);

        item.Title = "Updated";
        item.IsCompleted = true;
        _service.Update(item);

        var items = _service.GetAll();
        Assert.Single(items);
        Assert.Equal("Updated", items[0].Title);
        Assert.True(items[0].IsCompleted);
    }

    [Fact]
    public void Delete_ShouldRemoveItem()
    {
        var item = new TodoItem { Title = "To delete" };
        _service.Add(item);
        var id = item.Id;

        _service.Delete(id);

        Assert.Empty(_service.GetAll());
    }

    [Fact]
    public void Delete_ShouldNotThrow_WhenIdDoesNotExist()
    {
        _service.Delete(999);
    }

    [Fact]
    public void Add_ShouldCreateMultipleItems()
    {
        for (int i = 0; i < 10; i++)
            _service.Add(new TodoItem { Title = $"Task {i}" });

        Assert.Equal(10, _service.GetAll().Count);
    }

    [Fact]
    public void Initialize_ShouldNotThrow_WhenCalledMultipleTimes()
    {
        _service.Initialize();
        _service.Initialize();
        _service.Add(new TodoItem { Title = "Works" });
        Assert.NotEmpty(_service.GetAll());
    }
}
