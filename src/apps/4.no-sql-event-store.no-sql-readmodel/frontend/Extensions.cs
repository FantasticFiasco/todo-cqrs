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
