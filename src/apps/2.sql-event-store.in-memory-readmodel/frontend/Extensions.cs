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
            var connectionString = BuildConnectionString(configuration);
            var eventStore = new SqlEventStore(connectionString);
            var readModel = new InMemoryTodoList();

            var messageDispatcher = new MessageDispatcher(eventStore);
            messageDispatcher.ScanInstance(readModel);
            messageDispatcher.ScanInstance(new TodoAggregate());

            self.AddSingleton<ITodoList>(_ => readModel);
            self.AddSingleton(_ => messageDispatcher);
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            var host = configuration["DB_HOST"];
            var username = configuration["DB_USER"];
            var password = configuration["DB_PASSWORD"];

            return $"Host={host};Username={username};Password={password}";
        }
    }
}