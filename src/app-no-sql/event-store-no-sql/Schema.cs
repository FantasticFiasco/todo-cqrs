using MongoDB.Bson.Serialization;

namespace EventStore.NoSql
{
    public static class Schema
    {
        public static void Create()
        {
            BsonClassMap.RegisterClassMap<Event>();
        }
    }
}
