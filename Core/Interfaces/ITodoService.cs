using System.Collections.Generic;
using RigorStarter.Shared.Models;

namespace RigorStarter.Core.Interfaces;

public interface ITodoService
{
    void Initialize();
    List<TodoItem> GetAll();
    void Add(TodoItem item);
    void Update(TodoItem item);
    void Delete(int id);
}
