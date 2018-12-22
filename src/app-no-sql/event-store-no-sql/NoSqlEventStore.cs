using System;
using System.Collections.Generic;
using Cqrs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventStore.NoSql
{
    public class NoSqlEventStore : IEventStore
    {
        private readonly IMongoDatabase database;
        private readonly BsonDocument filter;

        public NoSqlEventStore()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("event");
            filter = new BsonDocument();
        }


        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id)
        {
            var collection = database.GetCollection<BsonDocument>(id.ToString());

            // TODO: The items returned are unordered, please fix
            return collection.Find(filter).ToList();
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents) => throw new NotImplementedException();
    }
}
