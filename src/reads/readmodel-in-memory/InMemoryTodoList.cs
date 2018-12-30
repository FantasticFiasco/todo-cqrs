using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cqrs;
using Todo.Events;

namespace ReadModel.InMemory
{
    public class InMemoryTodoList :
        ITodoList,
        ISubscribeTo<TodoAdded>,
        ISubscribeTo<TodoRenamed>,
        ISubscribeTo<TodoCompleted>,
        ISubscribeTo<TodoIncompleted>,
        ISubscribeTo<TodoRemoved>
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

        public void Handle(TodoAdded e)
        {
            var todoItem = new TodoItem(e.Id, e.Title, false);

            todoItemById.Add(todoItem.Id, todoItem);
        }

        public void Handle(TodoRenamed e)
        {
            todoItemById[e.Id].Title = e.NewTitle;
        }

        public void Handle(TodoCompleted e)
        {
            todoItemById[e.Id].IsCompleted = true;
        }

        public void Handle(TodoIncompleted e)
        {
            todoItemById[e.Id].IsCompleted = false;
        }

        public void Handle(TodoRemoved e)
        {
            todoItemById.Remove(e.Id);
        }
    }
}
