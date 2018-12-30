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
            var eventStore = BuildEventStore(configuration);
            var eventConsumer = BuildEventConsumer(configuration);
            var readModel = BuildReadModel(configuration);
            var messageDispatcher = BuildMessageDispatcher(eventStore, eventConsumer);

            self
                .AddSingleton(_ => messageDispatcher)
                .AddSingleton(_ => readModel);
        }

        private static IEventStore BuildEventStore(IConfiguration configuration)
        {
            var host = configuration["EVENT_STORE_HOST"];
            var username = configuration["EVENT_STORE_USER"];
            var password = configuration["EVENT_STORE_PASSWORD"];
            var connectionString = $"mongodb://{username}:{password}@{host}:27017";

            return new NoSqlEventStore(connectionString);
        }

        private static EventConsumer BuildEventConsumer(IConfiguration configuration)
        {
            var connectionString = BuildReadModelConnectionString(configuration);

            return new EventConsumer(connectionString);
        }

        private static ITodoList BuildReadModel(IConfiguration configuration)
        {
            var connectionString = BuildReadModelConnectionString(configuration);

            return new NoSqlTodoList(connectionString);
        }

        private static string BuildReadModelConnectionString(IConfiguration configuration)
        {
            var host = configuration["READ_MODEL_HOST"];
            var username = configuration["READ_MODEL_USER"];
            var password = configuration["READ_MODEL_PASSWORD"];

            return $"mongodb://{username}:{password}@{host}:27017";
        }

        private static MessageDispatcher BuildMessageDispatcher(IEventStore eventStore, EventConsumer eventConsumer)
        {
            var messageDispatcher = new MessageDispatcher(eventStore);

            // Let the message dispatcher scan the aggregate and register the IHandleCommand implementations
            messageDispatcher.ScanInstance(new TodoAggregate());

            // Let the message dispatcher scan the event consumer and register the ISubscribeTo implementations
            messageDispatcher.ScanInstance(eventConsumer);

            return messageDispatcher;
        }
    }
}
