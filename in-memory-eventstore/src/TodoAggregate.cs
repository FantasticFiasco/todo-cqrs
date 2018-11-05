using System.Collections;
using Cqrs;
using Todo.Commands;
using Todo.Events;

namespace InMemoryEventstore
{
    public class TodoAggregate : Aggregate,
        IHandleCommand<AddTodo>,
        IHandleCommand<RenameTodo>,
        IHandleCommand<CompleteTodo>,
        IHandleCommand<IncompleteTodo>,
        IHandleCommand<RemoveTodo>,
        IApplyEvent<TodoAdded>
    {
        public IEnumerable Handle(AddTodo command)
        {
            yield return new TodoAdded
            {
                Id = command.Id,
                Title = command.Title
            };
        }

        public IEnumerable Handle(RenameTodo command) => throw new System.NotImplementedException();

        public IEnumerable Handle(CompleteTodo command)
        {
            yield return new TodoCompleted
            {
                Id = command.Id
            };
        }

        public IEnumerable Handle(IncompleteTodo command) => throw new System.NotImplementedException();

        public IEnumerable Handle(RemoveTodo command) => throw new System.NotImplementedException();

        public void Apply(TodoAdded e)
        {
        }
    }
}
