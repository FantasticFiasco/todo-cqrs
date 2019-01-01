using Cqrs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ReadModel.NoSql.Private;
using Todo.Events;

namespace ReadModel.NoSql
{
    public class EventConsumer :
        ISubscribeTo<TodoAdded>,
        ISubscribeTo<TodoRenamed>,
        ISubscribeTo<TodoCompleted>,
        ISubscribeTo<TodoIncompleted>,
        ISubscribeTo<TodoRemoved>
    {
        private readonly MongoClient client;
        private readonly ILogger<EventConsumer> logger;

        public EventConsumer(string connectionString, ILogger<EventConsumer> logger)
        {
            client = new MongoClient(connectionString);
            this.logger = logger;
        }

        public void Handle(TodoAdded e)
        {
            logger.LogInformation("Consume todo added event with body {body}", e);

            var item = new TodoItem(e.Id, e.Title, false);

            client
                .GetCollection()
                .InsertOne(item);
        }

        public void Handle(TodoRenamed e)
        {
            logger.LogInformation("Consume todo renamed event with body {body}", e);

            client
                .GetCollection()
                .UpdateOne(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.Title, e.NewTitle));
        }

        public void Handle(TodoCompleted e)
        {
            logger.LogInformation("Consume todo completed event with body {body}", e);

            client
                .GetCollection()
                .UpdateOne(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.IsCompleted, true));
        }

        public void Handle(TodoIncompleted e)
        {
            logger.LogInformation("Consume todo incompleted event with body {body}", e);

            client
                .GetCollection()
                .UpdateOne(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.IsCompleted, false));
        }

        public void Handle(TodoRemoved e)
        {
            logger.LogInformation("Consume todo removed event with body {body}", e);

            client
                .GetCollection()
                .DeleteOne(Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id));
        }
    }
}
