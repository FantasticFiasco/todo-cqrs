using System;
using MongoDB.Bson.Serialization;

namespace EventStore.NoSql
{
    public class Schema
    {
        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public static void Create()
        {
            BsonClassMap.RegisterClassMap<Schema>();
        }
    }
}
