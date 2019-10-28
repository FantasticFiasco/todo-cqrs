using System.Threading.Tasks;
using GraphQL.Configuration;
using Todo;

namespace Frontend
{
    public class Mutation : IMutation
    {
        private readonly MutationClient client;

        public Mutation(MutationClient client)
        {
            this.client = client;
        }

        public Task HandleAddAsync(AddTodo command)
        {
            return client.HandleAddAsync(command);
        }

        public Task HandleRenameAsync(RenameTodo command)
        {
            return client.HandleRenameAsync(command);
        }

        public Task HandleCompleteAsync(CompleteTodo command)
        {
            return client.HandleCompleteAsync(command);
        }

        public Task HandleIncompleteAsync(IncompleteTodo command)
        {
            return client.HandleIncompleteAsync(command);
        }

        public Task HandleRemoveAsync(RemoveTodo command)
        {
            return client.HandleRemoveAsync(command);
        }
    }
}
