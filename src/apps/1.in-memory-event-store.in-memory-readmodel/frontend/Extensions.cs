using Cqrs;
using EventStore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.InMemory;
using Todo;

namespace Frontend
{
    public static class Extensions
    {
        public static void AddCqrs(this IServiceCollection self)
        {
            // Write model
            self.AddSingleton<IEventStore, InMemoryEventStore>();

            // Read model
            self
                .AddSingleton<InMemoryEventProcessor>()
                .AddSingleton<InMemoryTodoList>()
                // Workaround to resolve the same Singleton instance using both its type and its
                // implemented interface
                .AddSingleton<ITodoList>(provider => provider.GetService<InMemoryTodoList>());

            // Command relay
            self.AddSingleton<ICommandRelay>(provider =>
            {
                // Create the command relay
                var eventStore = provider.GetService<IEventStore>();
                var commandRelay = new CommandRelay(eventStore);

                // Let the command relay scan the aggregate and register command handlers
                commandRelay.RegisterHandlersFor<TodoAggregate>();

                // Let the command relay scan the event processor and register publishers
                var eventProcessor = provider.GetService<InMemoryEventProcessor>();
                commandRelay.RegisterPublishersFor(eventProcessor);

                return commandRelay;
            });
        }
    }
}
