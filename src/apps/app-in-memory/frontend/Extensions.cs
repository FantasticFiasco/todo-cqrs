using Cqrs;
using EventStore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using ReadModel.InMemory;
using Todo;

namespace Frontend
{
    public static class Extensions
    {
        public static void AddCqrs(this IServiceCollection self)
        {
            var todoList = new TodoList();

            var messageDispatcher = new MessageDispatcher(new InMemoryEventStore());
            messageDispatcher.ScanInstance(todoList);
            messageDispatcher.ScanInstance(new TodoAggregate());

            self.AddSingleton<ITodoList>(_ => todoList);
            self.AddSingleton(_ => messageDispatcher);
        }
    }
}
