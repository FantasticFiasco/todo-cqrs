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
                .AddSingleton<EventConsumer>()
                .AddSingleton<InMemoryTodoList>()
                .AddSingleton<ITodoList>(provider => provider.GetService<InMemoryTodoList>());

            // Message dispatcher
            self.AddSingleton(provider =>
            {
                // Create the message dispatcher
                var eventStore = provider.GetService<IEventStore>();
                var messageDispatcher = new MessageDispatcher(eventStore);

                // Let the message dispatcher scan the aggregate and register IHandleCommand implementations
                messageDispatcher.ScanInstance(new TodoAggregate());

                // Let the message dispatcher scan the event consumer and register ISubscribeTo implementations
                var eventConsumer = provider.GetService<EventConsumer>();
                messageDispatcher.ScanInstance(eventConsumer);

                return messageDispatcher;
            });
        }
    }
}
