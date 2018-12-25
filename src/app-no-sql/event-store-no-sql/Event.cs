using System;
using MongoDB.Bson;

namespace EventStore.NoSql
{
    public class Event
    {
        public int Version { get; set; }

        public string Type { get; set; }

        public BsonDocument Body { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
