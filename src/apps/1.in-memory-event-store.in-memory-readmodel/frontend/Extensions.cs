using Cqrs;
using EventStore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.InMemory;
using Todo;

namespace Frontend
{
    public static class Extensions
    {
        public static void AddCqrs(this IServiceCollection self)
        {
            var eventStore = new InMemoryEventStore();
            var readModel = new InMemoryTodoList();

            var messageDispatcher = new MessageDispatcher(eventStore);
            messageDispatcher.ScanInstance(readModel);
            messageDispatcher.ScanInstance(new TodoAggregate());

            self.AddSingleton<ITodoList>(_ => readModel);
            self.AddSingleton(_ => messageDispatcher);
        }
    }
}
