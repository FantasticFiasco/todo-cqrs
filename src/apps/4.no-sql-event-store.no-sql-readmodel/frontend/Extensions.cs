using Cqrs;
using EventStore.NoSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.NoSql;
using Todo;

namespace Frontend
{
    public static class Extensions
    {
        public static void AddDatabase(this IServiceCollection _)
        {
            EventStore.NoSql.Schema.Create();
            ReadModel.NoSql.Schema.Create();
        }

        public static void AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            // Write model
            self
                .AddSingleton(_ => BuildEventStoreConnectionString(configuration))
                .AddSingleton<IEventStore, NoSqlEventStore>();

            // Read model
            self
                .AddSingleton(_ => BuildReadModelConnectionString(configuration))
                .AddSingleton<EventConsumer>()
                .AddSingleton<ITodoList, NoSqlTodoList>();

            // Command relay
            self.AddSingleton<ICommandRelay>(provider =>
            {
                // Create the command relay
                var eventStore = provider.GetService<IEventStore>();
                var commandRelay = new CommandRelay(eventStore);

                // Let the command relay scan the aggregate and register command handlers
                commandRelay.RegisterHandlersFor<TodoAggregate>();

                // Let the command relay scan the event consumer and register publishers
                var eventConsumer = provider.GetService<EventConsumer>();
                commandRelay.RegisterPublishersFor(eventConsumer);

                return commandRelay;
            });
        }

        private static EventStore.NoSql.ConnectionString BuildEventStoreConnectionString(IConfiguration configuration)
        {
            var host = configuration["EVENT_STORE_HOST"];
            var username = configuration["EVENT_STORE_USER"];
            var password = configuration["EVENT_STORE_PASSWORD"];

            return new EventStore.NoSql.ConnectionString(host, username, password);
        }

        private static ReadModel.NoSql.ConnectionString BuildReadModelConnectionString(IConfiguration configuration)
        {
            var host = configuration["READ_MODEL_HOST"];
            var username = configuration["READ_MODEL_USER"];
            var password = configuration["READ_MODEL_PASSWORD"];

            return new ReadModel.NoSql.ConnectionString(host, username, password);
        }
    }
}
