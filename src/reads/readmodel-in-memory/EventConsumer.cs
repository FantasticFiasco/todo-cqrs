using Cqrs;
using Todo.Events;

namespace ReadModel.InMemory
{
    /// <summary>
    /// Class responsible for consuming events published by the aggregates, and update the read
    /// model accordingly.
    /// </summary>
    public class EventConsumer :
        ISubscribeTo<TodoAdded>,
        ISubscribeTo<TodoRenamed>,
        ISubscribeTo<TodoCompleted>,
        ISubscribeTo<TodoIncompleted>,
        ISubscribeTo<TodoRemoved>
    {
        private readonly InMemoryTodoList todoList;

        public EventConsumer(InMemoryTodoList todoList)
        {
            this.todoList = todoList;
        }

        public void Handle(TodoAdded e)
        {
            todoList.Add(e.Id, e.Title);
        }

        public void Handle(TodoRenamed e)
        {
            todoList.Rename(e.Id, e.NewTitle);
        }

        public void Handle(TodoCompleted e)
        {
            todoList.SetCompleted(e.Id, true);
        }

        public void Handle(TodoIncompleted e)
        {
            todoList.SetCompleted(e.Id, false);
        }

        public void Handle(TodoRemoved e)
        {
            todoList.Remove(e.Id);
        }
    }
}
