using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cqrs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventStore.NoSql
{
    public class NoSqlEventStore : IEventStore
    {
        private readonly IMongoDatabase database;
        private readonly BsonDocument filter;

        public NoSqlEventStore(string connectionString)
        {
            var client = new MongoClient(connectionString);
            database = client.GetDatabase("event");
            filter = new BsonDocument();
        }

        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id)
        {
            var collection = database.GetCollection<Schema>(id.ToString());

            // TODO: The items returned are unordered, please fix
            return collection.Find(filter).ToList();
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents) =>
            throw new NotImplementedException();
    }
}
