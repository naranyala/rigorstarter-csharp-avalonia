using System;
using System.IO;
using RigorStarter.Core.Services;
using RigorStarter.Shared.Models;
using Xunit;

namespace RigorStarter.Tests;

public class TodoServiceJsonTests : IDisposable
{
    private readonly string _filePath;
    private readonly TodoServiceJson _service;

    public TodoServiceJsonTests()
    {
        _filePath = Path.Combine(Path.GetTempPath(), $"todos_json_test_{Guid.NewGuid()}.json");
        _service = new TodoServiceJson(_filePath);
        _service.Initialize();
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
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
        var item = new TodoItem { Title = "Test task" };
        _service.Add(item);

        Assert.True(item.Id > 0);

        var items = _service.GetAll();
        Assert.Single(items);
        Assert.Equal("Test task", items[0].Title);
    }

    [Fact]
    public void GetAll_ShouldReturnItemsInDescendingOrder()
    {
        _service.Add(new TodoItem { Title = "First", CreatedAt = new DateTime(2024, 1, 1) });
        _service.Add(new TodoItem { Title = "Second", CreatedAt = new DateTime(2024, 1, 2) });
        _service.Add(new TodoItem { Title = "Third", CreatedAt = new DateTime(2024, 1, 3) });

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
    public void Initialize_ShouldLoadExistingData()
    {
        _service.Add(new TodoItem { Title = "Persisted" });
        _service.Add(new TodoItem { Title = "Also persisted" });

        // Create a new service instance pointing at the same file
        var service2 = new TodoServiceJson(_filePath);
        service2.Initialize();

        var items = service2.GetAll();
        Assert.Equal(2, items.Count);
        Assert.Contains(items, i => i.Title == "Persisted");
    }

    [Fact]
    public void Add_And_Delete_ShouldPersistToFile()
    {
        var item = new TodoItem { Title = "Persistent" };
        _service.Add(item);
        Assert.True(File.Exists(_filePath));

        var content = File.ReadAllText(_filePath);
        Assert.Contains("Persistent", content);
    }
}
