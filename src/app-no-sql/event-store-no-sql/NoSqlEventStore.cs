using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace EventStore.NoSql
{
    public class NoSqlEventStore : IEventStore
    {
        private const string DatabaseName = "event";

        private readonly MongoClient client;

        public NoSqlEventStore(string connectionString)
        {
            client = new MongoClient(connectionString);
        }

        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id)
        {
            // TODO: The items returned are unordered, please fix
            return GetCollection(id)
                .Find(FilterDefinition<Event>.Empty)
                .ToList()
                .Select(e => Deserialize(e.Body, e.Type));
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents)
        {
            GetCollection(id)
                .InsertMany(
                    newEvents.Select((e, i) => new Event
                    {
                        Version = eventsLoaded + i,
                        Type = e.GetType().AssemblyQualifiedName,
                        Body = Serialize(e),
                        CreatedAt = DateTime.UtcNow
                    }));
        }

        private IMongoCollection<Event> GetCollection(Guid id)
        {
            var database = client.GetDatabase(DatabaseName);
            return database.GetCollection<Event>(id.ToString());
        }

        private static BsonDocument Serialize(object obj) =>
            BsonDocument.Create(obj);

        private static object Deserialize(BsonDocument body, string typeName) =>
            BsonSerializer.Deserialize(body, Type.GetType(typeName));
    }
}
