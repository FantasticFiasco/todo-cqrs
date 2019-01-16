using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ReadModel.NoSql.Private;

namespace ReadModel.NoSql
{
    public class NoSqlTodoList : ITodoList, ITodoListSynchronizer
    {
        private readonly MongoClient client;
        private readonly ILogger<NoSqlTodoList> logger;

        public NoSqlTodoList(ConnectionString connectionString, ILogger<NoSqlTodoList> logger)
        {
            client = new MongoClient(connectionString);
            this.logger = logger;
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

        public void Add(Guid id, string title)
        {
            logger.LogInformation("Add {id} with {title}", id, title);

            var item = new TodoItem
            {
                Id = id,
                Title = title
            };

            client
                .GetCollection()
                .InsertOne(item);
        }

        public void Rename(Guid id, string newTitle)
        {
            logger.LogInformation("Rename {id} to {newTitle}", id, newTitle);

            client
                .GetCollection()
                .FindOneAndUpdate(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, id),
                    Builders<TodoItem>.Update.Set(item => item.Title, newTitle));
        }

        public void SetCompleted(Guid id, bool isCompleted)
        {
            logger.LogInformation("Set {id} to {isCompleted}", id, isCompleted ? "completed" : "not completed");

            client
                .GetCollection()
                .FindOneAndUpdate(
                    Builders<TodoItem>.Filter.Eq(item => item.Id, id),
                    Builders<TodoItem>.Update.Set(item => item.IsCompleted, isCompleted));
        }

        public void Remove(Guid id)
        {
            logger.LogInformation("Remove {id}", id);

            client
                .GetCollection()
                .FindOneAndDelete(Builders<TodoItem>.Filter.Eq(item => item.Id, id));
        }
    }
}
