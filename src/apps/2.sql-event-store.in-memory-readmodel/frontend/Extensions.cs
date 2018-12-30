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
            var connectionString = BuildConnectionString(configuration);
            var schema = new Schema(connectionString);
            schema.Create();
        }

        public static void AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            var eventStore = BuildEventStore(configuration);
            var readModel = BuildReadModel();
            var messageDispatcher = BuildMessageDispatcher(eventStore, readModel);

            self
                .AddSingleton(_ => messageDispatcher)
                .AddSingleton(_ => readModel);
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            var host = configuration["EVENT_STORE_HOST"];
            var username = configuration["EVENT_STORE_USER"];
            var password = configuration["EVENT_STORE_PASSWORD"];

            return $"Host={host};Username={username};Password={password}";
        }

        private static IEventStore BuildEventStore(IConfiguration configuration)
        {
            var connectionString = BuildConnectionString(configuration);
            return new SqlEventStore(connectionString);
        }

        private static ITodoList BuildReadModel()
        {
            return new InMemoryTodoList();
        }

        private static MessageDispatcher BuildMessageDispatcher(IEventStore eventStore, ITodoList readModel)
        {
            var messageDispatcher = new MessageDispatcher(eventStore);

            // Let the message dispatcher scan the aggregate and register the IHandleCommand implementations
            messageDispatcher.ScanInstance(new TodoAggregate());

            // Let the message dispatcher scan the event consumer and register the ISubscribeTo implementations
            messageDispatcher.ScanInstance(new EventConsumer((InMemoryTodoList)readModel));

            return messageDispatcher;
        }
    }
}
