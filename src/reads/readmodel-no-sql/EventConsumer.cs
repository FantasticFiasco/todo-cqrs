using Cqrs;
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

        public EventConsumer(string connectionString)
        {
            client = new MongoClient(connectionString);
        }

        public void Handle(TodoAdded e)
        {
            var item = new TodoItem(e.Id, e.Title, false);

            client
                .GetCollection()
                .InsertOne(item);
        }

        public void Handle(TodoRenamed e)
        {
            client
                .GetCollection()
                .UpdateOne(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.Title, e.NewTitle));
        }

        public void Handle(TodoCompleted e)
        {
            client
                .GetCollection()
                .UpdateOne(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.IsCompleted, true));
        }

        public void Handle(TodoIncompleted e)
        {
            client
                .GetCollection()
                .UpdateOne(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id),
                    Builders<TodoItem>.Update.Set(item => item.IsCompleted, false));
        }

        public void Handle(TodoRemoved e)
        {
            client
                .GetCollection()
                .DeleteOne(Builders<TodoItem>.Filter.Eq(item => item.Id, e.Id));
        }
    }
}
