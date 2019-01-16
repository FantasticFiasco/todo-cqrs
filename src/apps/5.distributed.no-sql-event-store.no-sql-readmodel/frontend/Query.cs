using System;
using System.Threading.Tasks;
using GraphQL.Configuration;
using ReadModel;

namespace Frontend
{
    public class Query : IQuery
    {
        private readonly QueryClient client;

        public Query(QueryClient client)
        {
            this.client = client;
        }

        public Task<TodoItem[]> GetAllAsync()
        {
            return client.GetAllAsync();
        }

        public Task<TodoItem> GetAsync(Guid id)
        {
            return client.GetAsync(id);
        }
    }
}
