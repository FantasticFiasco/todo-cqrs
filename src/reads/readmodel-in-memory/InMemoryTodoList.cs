using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ReadModel.InMemory
{
    /// <summary>
    /// In-memory implementation of <see cref="ITodoList"/>.
    /// </summary>
    public class InMemoryTodoList : ITodoList, ITodoListSynchronizer
    {
        private readonly Dictionary<Guid, TodoItem> todoItemById;
        private readonly ILogger<InMemoryTodoList> logger;

        public InMemoryTodoList(ILogger<InMemoryTodoList> logger)
        {
            todoItemById = new Dictionary<Guid, TodoItem>();
            this.logger = logger;
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

        public void Add(Guid id, string title)
        {
            logger.LogInformation("Add {id} with {title}", id, title);

            var item = new TodoItem
            {
                Id = id,
                Title = title
            };

            todoItemById.Add(item.Id, item);
        }

        public void Rename(Guid id, string newTitle)
        {
            logger.LogInformation("Rename {id} to {newTitle}", id, newTitle);

            todoItemById[id].Title = newTitle;
        }

        public void SetCompleted(Guid id, bool isCompleted)
        {
            logger.LogInformation("Set {id} to {isCompleted}", id, isCompleted ? "completed" : "not completed");

            todoItemById[id].IsCompleted = isCompleted;
        }

        public void Remove(Guid id)
        {
            logger.LogInformation("Remove {id}", id);

            todoItemById.Remove(id);
        }
    }
}
