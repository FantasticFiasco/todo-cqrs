using System.Threading.Tasks;
using Todo;

namespace GraphQL.Configuration
{
    public interface IMutation
    {
        Task HandleAddAsync(AddTodo command);

        Task HandleRenameAsync(RenameTodo command);

        Task HandleCompleteAsync(CompleteTodo command);

        Task HandleIncompleteAsync(IncompleteTodo command);

        Task HandleRemoveAsync(RemoveTodo command);
    }
}
