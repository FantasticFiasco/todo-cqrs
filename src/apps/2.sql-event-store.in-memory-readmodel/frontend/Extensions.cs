using Cqrs;
using EventStore.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.InMemory;
using Todo;

namespace Frontend
{
    public static class Extensions
    {
        public static void AddDatabase(this IServiceCollection _, IConfiguration configuration)
        {
            var connectionString = BuildEventStoreConnectionString(configuration);
            var schema = new Schema(connectionString);
            schema.Create();
        }

        public static void AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            // Write model
            self
                .AddSingleton(_ => BuildEventStoreConnectionString(configuration))
                .AddSingleton<IEventStore, SqlEventStore>();

            // Read model
            self
                .AddSingleton<EventConsumer>()
                .AddSingleton<InMemoryTodoList>()
                // Workaround to resolve the same Singleton instance using both its type and its
                // implemented interface
                .AddSingleton<ITodoList>(provider => provider.GetService<InMemoryTodoList>());

            // Command relay
            self.AddSingleton(provider =>
            {
                // Create the command relay
                var eventStore = provider.GetService<IEventStore>();
                var commandRelay = new CommandRelay(eventStore);

                // Let the command relay scan the aggregate and register IHandleCommand<T> implementations
                commandRelay.AddHandlersFor<TodoAggregate>();

                // Let the command relay scan the event consumer and register ISubscribeTo implementations
                var eventConsumer = provider.GetService<EventConsumer>();
                commandRelay.ScanInstance(eventConsumer);

                return commandRelay;
            });
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
