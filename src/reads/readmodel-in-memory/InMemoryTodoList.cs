using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadModel.InMemory
{
    /// <summary>
    /// In-memory implementation of <see cref="ITodoList"/>.
    /// </summary>
    public class InMemoryTodoList : ITodoList
    {
        private readonly Dictionary<Guid, TodoItem> todoItemById;

        public InMemoryTodoList()
        {
            todoItemById = new Dictionary<Guid, TodoItem>();
        }

        public Task<TodoItem[]> GetAllAsync()
        {
            return Task.FromResult(todoItemById.Values.ToArray());
        }

        public Task<TodoItem> GetAsync(Guid id)
        {
            todoItemById.TryGetValue(id, out var todoItem);

            return Task.FromResult(todoItem);
        }

        internal void Add(Guid id, string title)
        {
            var item = new TodoItem(id, title, false);

            todoItemById.Add(item.Id, item);
        }

        internal void Rename(Guid id, string newTitle)
        {
            todoItemById[id].Title = newTitle;
        }

        internal void SetCompleted(Guid id, bool isCompleted)
        {
            todoItemById[id].IsCompleted = isCompleted;
        }

        internal void Remove(Guid id)
        {
            todoItemById.Remove(id);
        }
    }
}
