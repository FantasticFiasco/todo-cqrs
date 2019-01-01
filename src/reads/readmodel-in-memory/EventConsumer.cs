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
            logger.LogInformation("Consume todo added event with body {body}", e);

            todoList.Add(e.Id, e.Title);
        }

        public void Handle(TodoRenamed e)
        {
            logger.LogInformation("Consume todo renamed event with body {body}", e);

            todoList.Rename(e.Id, e.NewTitle);
        }

        public void Handle(TodoCompleted e)
        {
            logger.LogInformation("Consume todo completed event with body {body}", e);

            todoList.SetCompleted(e.Id, true);
        }

        public void Handle(TodoIncompleted e)
        {
            logger.LogInformation("Consume todo incompleted event with body {body}", e);

            todoList.SetCompleted(e.Id, false);
        }

        public void Handle(TodoRemoved e)
        {
            logger.LogInformation("Consume todo removed event with body {body}", e);

            todoList.Remove(e.Id);
        }
    }
}
