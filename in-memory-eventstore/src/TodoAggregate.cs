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
        IApplyEvent<TodoAdded>,
        IApplyEvent<TodoCompleted>
    {
        public IEnumerable Handle(AddTodo command)
        {
            yield return new TodoAdded(command.Id, command.Title);
        }

        public IEnumerable Handle(RenameTodo command) => throw new System.NotImplementedException();

        public IEnumerable Handle(CompleteTodo command)
        {
            yield return new TodoCompleted(command.Id);
        }

        public IEnumerable Handle(IncompleteTodo command)
        {
            yield return new TodoIncompleted(command.Id);
        }

        public IEnumerable Handle(RemoveTodo command)
        {
            yield return new TodoRemoved(command.Id);
        }

        public void Apply(TodoAdded e)
        {
        }

        public void Apply(TodoCompleted e)
        {
        }
    }
}
