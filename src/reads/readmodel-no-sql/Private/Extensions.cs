using MongoDB.Driver;

namespace ReadModel.NoSql.Private
{
    internal static class Extensions
    {
        private const string DatabaseName = "todo";
        private const string CollectionName = "item";

        internal static IMongoCollection<TodoItem> GetCollection(this MongoClient self)
        {
            var database = self.GetDatabase(DatabaseName);
            return database.GetCollection<TodoItem>(CollectionName);
        }
    }
}
