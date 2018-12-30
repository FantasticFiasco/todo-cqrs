using System;
using System.Threading.Tasks;

namespace ReadModel
{
    public interface ITodoList
    {
        Task<TodoItem[]> GetAllAsync();

        Task<TodoItem> GetAsync(Guid id);
    }
}
