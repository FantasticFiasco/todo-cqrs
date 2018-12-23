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

        public async Task<IEnumerable<object>> LoadEventsForAsync<TAggregate>(Guid id)
        {
            var collection = database.GetCollection<Schema>(id.ToString());

            // TODO: The items returned are unordered, please fix
            return await collection.Find(filter).ToListAsync();
        }

        public Task SaveEventsForAsync<TAggregate>(Guid id, int version, object[] newEvents) =>
            throw new NotImplementedException();
    }
}
