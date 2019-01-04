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
        IPublisher<TodoAdded>,
        IPublisher<TodoRenamed>,
        IPublisher<TodoCompleted>,
        IPublisher<TodoIncompleted>,
        IPublisher<TodoRemoved>
    {
        private readonly InMemoryTodoList todoList;
        private readonly ILogger<EventConsumer> logger;

        public EventConsumer(InMemoryTodoList todoList, ILogger<EventConsumer> logger)
        {
            this.todoList = todoList;
            this.logger = logger;
        }

        public void Publish(TodoAdded e)
        {
            Log(e);

            todoList.Add(e.Id, e.Title);
        }

        public void Publish(TodoRenamed e)
        {
            Log(e);

            todoList.Rename(e.Id, e.NewTitle);
        }

        public void Publish(TodoCompleted e)
        {
            Log(e);

            todoList.SetCompleted(e.Id, true);
        }

        public void Publish(TodoIncompleted e)
        {
            Log(e);

            todoList.SetCompleted(e.Id, false);
        }

        public void Publish(TodoRemoved e)
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
