using Cqrs;
using EventStore.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.ReadModel;

namespace Todo
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
            var todoList = new TodoList();

            var messageDispatcher = new MessageDispatcher(eventStore);
            messageDispatcher.ScanInstance(todoList);
            messageDispatcher.ScanInstance(new TodoAggregate());

            self.AddSingleton<ITodoList>(_ => todoList);
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
