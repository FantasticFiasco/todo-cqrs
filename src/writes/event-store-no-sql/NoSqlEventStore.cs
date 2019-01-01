using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace EventStore.NoSql
{
    public class NoSqlEventStore : IEventStore
    {
        private const string DatabaseName = "event";

        private readonly MongoClient client;
        private readonly ILogger<NoSqlEventStore> logger;

        public NoSqlEventStore(string connectionString, ILogger<NoSqlEventStore> logger)
        {
            client = new MongoClient(connectionString);
            this.logger = logger;
        }

        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id)
        {
            var events = GetCollection(id)
                .Find(FilterDefinition<Event>.Empty)
                .Sort("{version: 1}")
                .ToList()
                .Select(e => BsonSerializer.Deserialize(e.Body, Type.GetType(e.Type)))
                .ToArray();

            logger.LogInformation("Loaded {count} events for aggregate {id}", events.Length, id);

            return events;
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents)
        {
            logger.LogInformation("Save {count} event(s) for aggregate with id {id}", newEvents.Length, id);

            GetCollection(id)
                .InsertMany(
                    newEvents.Select((e, i) => new Event
                    {
                        Version = eventsLoaded + i,
                        Type = e.GetType().AssemblyQualifiedName,
                        Body = e.ToBsonDocument(),
                        CreatedAt = DateTime.UtcNow
                    }));
        }

        private IMongoCollection<Event> GetCollection(Guid id)
        {
            var database = client.GetDatabase(DatabaseName);
            return database.GetCollection<Event>(id.ToString());
        }
    }
}
