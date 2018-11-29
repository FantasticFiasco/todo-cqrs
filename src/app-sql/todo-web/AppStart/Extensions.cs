using Cqrs;
using EventStore.Sql;
using Microsoft.Extensions.DependencyInjection;
using Todo.ReadModel;

// ReSharper disable once CheckNamespace
namespace Todo.Web
{
    public static class Extensions
    {
        private const string ConnectionString = "Host=sql;Username=root;Password=secret";

        public static void AddCqrs(this IServiceCollection self)
        {
            var todoList = new TodoList();

            var messageDispatcher = new MessageDispatcher(new SqlEventStore(ConnectionString));
            messageDispatcher.ScanInstance(todoList);
            messageDispatcher.ScanInstance(new TodoAggregate());

            self.AddSingleton<ITodoList>(_ => todoList);
            self.AddSingleton(_ => messageDispatcher);
        }

        public static void AddDatabase(this IServiceCollection _)
        {
            var schema = new Schema(ConnectionString);
            schema.Create();
        }
    }
}
