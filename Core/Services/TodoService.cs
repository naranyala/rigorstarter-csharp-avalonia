using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Models;

namespace RigorStarter.Core.Services;

public class TodoService : ITodoService
{
    private readonly string _connectionString;

    public TodoService(string? dbPath = null)
    {
        dbPath ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "persistent", "todos.db");
        var dir = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        _connectionString = $"Data Source={dbPath}";
    }

    public void Initialize()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
                CREATE TABLE IF NOT EXISTS todos (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    is_completed INTEGER NOT NULL DEFAULT 0,
                    created_at TEXT NOT NULL
                )
            """;
        cmd.ExecuteNonQuery();
    }

    public List<TodoItem> GetAll()
    {
        var items = new List<TodoItem>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT id, title, is_completed, created_at FROM todos ORDER BY created_at DESC";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            items.Add(
                new TodoItem
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    IsCompleted = reader.GetInt32(2) != 0,
                    CreatedAt = DateTime.Parse(reader.GetString(3)),
                }
            );
        }
        return items;
    }

    public void Add(TodoItem item)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
                INSERT INTO todos (title, is_completed, created_at)
                VALUES ($title, $is_completed, $created_at);
                SELECT last_insert_rowid()
            """;
        cmd.Parameters.AddWithValue("$title", item.Title);
        cmd.Parameters.AddWithValue("$is_completed", item.IsCompleted ? 1 : 0);
        cmd.Parameters.AddWithValue("$created_at", item.CreatedAt.ToString("O"));
        item.Id = Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Update(TodoItem item)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "UPDATE todos SET title = $title, is_completed = $is_completed WHERE id = $id";
        cmd.Parameters.AddWithValue("$title", item.Title);
        cmd.Parameters.AddWithValue("$is_completed", item.IsCompleted ? 1 : 0);
        cmd.Parameters.AddWithValue("$id", item.Id);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM todos WHERE id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}
