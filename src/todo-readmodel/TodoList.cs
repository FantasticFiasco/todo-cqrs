using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs;
using Todo.Events;

namespace Todo.ReadModel
{
    public class TodoList :
        ITodoList,
        ISubscribeTo<TodoAdded>,
        ISubscribeTo<TodoRenamed>,
        ISubscribeTo<TodoCompleted>,
        ISubscribeTo<TodoIncompleted>,
        ISubscribeTo<TodoRemoved>
    {
        private readonly Dictionary<Guid, TodoItem> todoItemById;

        public TodoList()
        {
            todoItemById = new Dictionary<Guid, TodoItem>();
        }

        public TodoItem[] GetAll()
        {
            return todoItemById.Values.ToArray();
        }

        public void Handle(TodoAdded e)
        {
            var todoItem = new TodoItem(e.Id, e.Title);

            todoItemById.Add(todoItem.Id, todoItem);
        }

        public void Handle(TodoRenamed e) => throw new System.NotImplementedException();

        public void Handle(TodoCompleted e)
        {
            todoItemById[e.Id].IsCompleted = true;
        }

        public void Handle(TodoIncompleted e) => throw new System.NotImplementedException();

        public void Handle(TodoRemoved e) => throw new System.NotImplementedException();
    }
}
