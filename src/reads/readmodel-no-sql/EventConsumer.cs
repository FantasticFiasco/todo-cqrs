using Cqrs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ReadModel.NoSql.Private;
using Todo.Events;

namespace ReadModel.NoSql
{
    public class EventConsumer :
        IPublisher<TodoAdded>,
        IPublisher<TodoRenamed>,
        IPublisher<TodoCompleted>,
        IPublisher<TodoIncompleted>,
        IPublisher<TodoRemoved>
    {
        private readonly MongoClient client;
        private readonly ILogger<EventConsumer> logger;

        public EventConsumer(ConnectionString connectionString, ILogger<EventConsumer> logger)
        {
            client = new MongoClient(connectionString);
            this.logger = logger;
        }

        public void Publish(TodoAdded e)
        {
            Log(e);

            var item = new TodoItem(e.Id, e.Title, false);

            client
                .GetCollection()
                .InsertOne(item);
        }

        public void Publish(TodoRenamed e)
        {
            Log(e);

            client
                .GetCollection()
                .FindOneAndUpdate(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.Title, e.NewTitle));
        }

        public void Publish(TodoCompleted e)
        {
            Log(e);

            client
                .GetCollection()
                .FindOneAndUpdate(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.IsCompleted, true));
        }

        public void Publish(TodoIncompleted e)
        {
            Log(e);

            client
                .GetCollection()
                .FindOneAndUpdate(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.IsCompleted, false));
        }

        public void Publish(TodoRemoved e)
        {
            Log(e);

            client
                .GetCollection()
                .FindOneAndDelete(Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id));
        }

        private void Log<T>(T e)
        {
            logger.LogInformation("Consume {event}", e);
        }
    }
}
