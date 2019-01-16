using System;
using System.Threading.Tasks;
using GraphQL.Configuration;
using ReadModel;

namespace Frontend
{
    public class Query : IQuery
    {
        private readonly ITodoList readModel;

        public Query(ITodoList readModel)
        {
            this.readModel = readModel;
        }

        public Task<TodoItem[]> GetAllAsync()
        {
            return readModel.GetAllAsync();
        }

        public Task<TodoItem> GetAsync(Guid id)
        {
            return readModel.GetAsync(id);
        }
    }
}
