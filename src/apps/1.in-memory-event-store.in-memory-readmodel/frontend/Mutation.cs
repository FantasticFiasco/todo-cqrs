using System.Threading.Tasks;
using Cqrs;
using GraphQL.Configuration;
using Todo;

namespace Frontend
{
    public class Mutation : IMutation
    {
        private readonly ICommandRelay commandRelay;

        public Mutation(ICommandRelay commandRelay)
        {
            this.commandRelay = commandRelay;
        }

        public Task HandleAddAsync(AddTodo command)
        {
            commandRelay.SendCommand(command);

            return Task.CompletedTask;
        }

        public Task HandleRenameAsync(RenameTodo command)
        {
            commandRelay.SendCommand(command);

            return Task.CompletedTask;
        }

        public Task HandleCompleteAsync(CompleteTodo command)
        {
            commandRelay.SendCommand(command);

            return Task.CompletedTask;
        }

        public Task HandleIncompleteAsync(IncompleteTodo command)
        {
            commandRelay.SendCommand(command);

            return Task.CompletedTask;
        }

        public Task HandleRemoveAsync(RemoveTodo command)
        {
            commandRelay.SendCommand(command);

            return Task.CompletedTask;
        }
    }
}
