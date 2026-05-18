using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using RigorStarter.Shared.Models;

namespace RigorStarter.Core.Services;

public class TodoServiceJson
{
    private readonly string _filePath;
    private List<TodoItem> _items = new();
    private int _nextId = 1;

    public TodoServiceJson(string? filePath = null)
    {
        _filePath =
            filePath
            ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "persistent", "todos-json.json");
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    public void Initialize()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _items = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
            _nextId = _items.Count > 0 ? _items.Max(i => i.Id) + 1 : 1;
        }
    }

    public List<TodoItem> GetAll()
    {
        return _items.OrderByDescending(i => i.CreatedAt).ToList();
    }

    public void Add(TodoItem item)
    {
        item.Id = _nextId++;
        _items.Add(item);
        Save();
    }

    public void Update(TodoItem item)
    {
        var index = _items.FindIndex(i => i.Id == item.Id);
        if (index >= 0)
        {
            _items[index] = item;
            Save();
        }
    }

    public void Delete(int id)
    {
        _items.RemoveAll(i => i.Id == id);
        Save();
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(
            _items,
            new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(_filePath, json);
    }
}
