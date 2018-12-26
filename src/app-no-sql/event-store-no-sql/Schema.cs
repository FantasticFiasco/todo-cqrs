using MongoDB.Bson.Serialization;

namespace EventStore.NoSql
{
    public static class Schema
    {
        public static void Create()
        {
            BsonClassMap.RegisterClassMap<Event>(options =>
            {
                options.MapMember(from => from.Version).SetElementName("version");
                options.MapMember(from => from.Type).SetElementName("type");
                options.MapMember(from => from.Body).SetElementName("body");
                options.MapMember(from => from.CreatedAt).SetElementName("createdAt");
                options.SetIgnoreExtraElements(true);
            });
        }
    }
}
