using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using ReadModel.NoSql.Private;

namespace ReadModel.NoSql
{
    public class NoSqlTodoList : ITodoList
    {
        private readonly MongoClient client;

        public NoSqlTodoList(string connectionString)
        {
            client = new MongoClient(connectionString);
        }

        public async Task<TodoItem[]> GetAllAsync()
        {
            var items = await client
                .GetCollection()
                .Find(FilterDefinition<TodoItem>.Empty)
                .ToListAsync();

            return items.ToArray();
        }

        public async Task<TodoItem> GetAsync(Guid id)
        {
            return await client
                .GetCollection()
                .Find(Builders<TodoItem>.Filter.Eq(item => item.Id, id))
                .SingleOrDefaultAsync();
        }
    }
}
