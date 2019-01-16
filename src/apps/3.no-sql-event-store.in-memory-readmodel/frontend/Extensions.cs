using Cqrs;
using EventStore.NoSql;
using GraphQL.Configuration;
using Messaging.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.InMemory;
using Todo;

namespace Frontend
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection self)
        {
            Schema.Create();

            return self;
        }

        public static IServiceCollection AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            // Write model
            self
                .AddSingleton(_ => BuildEventStoreConnectionString(configuration))
                .AddSingleton<IEventStore, NoSqlEventStore>()
                .AddSingleton<InMemoryEventPublisher>();

            // Command relay
            self.AddSingleton<ICommandRelay>(provider =>
            {
                // Create the command relay
                var eventStore = provider.GetRequiredService<IEventStore>();
                var commandRelay = new CommandRelay(eventStore);

                // Let the command relay scan the aggregate and register its command handlers
                commandRelay.RegisterHandlersFor<TodoAggregate>();

                // Let the command relay scan the event publisher and register its events
                var publisher = provider.GetRequiredService<InMemoryEventPublisher>();
                commandRelay.RegisterPublishersFor(publisher);

                // Start consuming events by resolving the service
                provider.GetRequiredService<InMemoryEventConsumer>();

                return commandRelay;
            });

            // Read model
            self
                .AddSingleton<ITodoList, ITodoListSynchronizer, InMemoryTodoList>()
                .AddSingleton<InMemoryEventConsumer>();

            // GraphQL
            self
                .AddSingleton<IQuery, Query>()
                .AddSingleton<IMutation, Mutation>();

            return self;
        }

        private static ConnectionString BuildEventStoreConnectionString(IConfiguration configuration)
        {
            var host = configuration["EVENT_STORE_HOST"];
            var username = configuration["EVENT_STORE_USER"];
            var password = configuration["EVENT_STORE_PASSWORD"];

            return new ConnectionString(host, username, password);
        }
    }
}
