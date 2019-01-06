using System;
using System.Threading.Tasks;
using ReadModel;

namespace GraphQL.Configuration
{
    public interface IQuery
    {
        Task<TodoItem[]> GetAllAsync();

        Task<TodoItem> GetAsync(Guid id);
    }
}
