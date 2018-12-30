using MongoDB.Bson.Serialization;

namespace ReadModel.NoSql
{
    public static class Schema
    {
        public static void Create()
        {
            BsonClassMap.RegisterClassMap<TodoItem>(options =>
            {
                options.MapMember(from => from.Title).SetElementName("title");
                options.MapMember(from => from.IsCompleted).SetElementName("isCompleted");
            });
        }
    }
}
