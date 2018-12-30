using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ReadModel.NoSql
{
    public class NoSqlTodoList : ITodoList
    {
        private const string DatabaseName = "todo";
        private const string CollectionName = "item";

        private readonly MongoClient client;

        public NoSqlTodoList(string connectionString)
        {
            client = new MongoClient(connectionString);
        }

        public async Task<TodoItem[]> GetAllAsync()
        {
            var items = await GetCollection()
                .Find(FilterDefinition<TodoItem>.Empty)
                .ToListAsync();

            return items.ToArray();
        }

        public async Task<TodoItem> GetAsync(Guid id)
        {
            return await GetCollection()
                .Find(item => item.Id == id)
                .SingleOrDefaultAsync();
        }

        private IMongoCollection<TodoItem> GetCollection()
        {
            var database = client.GetDatabase(DatabaseName);
            return database.GetCollection<TodoItem>(CollectionName);
        }
    }
}
