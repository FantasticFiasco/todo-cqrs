using Cqrs;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EventConsumer> logger;

        public EventConsumer(InMemoryTodoList todoList, ILogger<EventConsumer> logger)
        {
            this.todoList = todoList;
            this.logger = logger;
        }

        public void Handle(TodoAdded e)
        {
            Log(e);

            todoList.Add(e.Id, e.Title);
        }

        public void Handle(TodoRenamed e)
        {
            Log(e);

            todoList.Rename(e.Id, e.NewTitle);
        }

        public void Handle(TodoCompleted e)
        {
            Log(e);

            todoList.SetCompleted(e.Id, true);
        }

        public void Handle(TodoIncompleted e)
        {
            Log(e);

            todoList.SetCompleted(e.Id, false);
        }

        public void Handle(TodoRemoved e)
        {
            Log(e);

            todoList.Remove(e.Id);
        }

        private void Log<T>(T e)
        {
            logger.LogInformation("Consume {event}", e);
        }
    }
}
