using System.Collections;
using Cqrs;
using Todo.Events;

namespace Todo
{
    public class TodoAggregate : Aggregate,
        IHandleCommand<AddTodo>,
        IHandleCommand<RenameTodo>,
        IHandleCommand<CompleteTodo>,
        IHandleCommand<IncompleteTodo>,
        IHandleCommand<RemoveTodo>,
        IApplyEvent<TodoAdded>,
        IApplyEvent<TodoRemoved>,
        IApplyEvent<TodoRenamed>,
        IApplyEvent<TodoCompleted>
    {
        private bool isRemoved;

        public IEnumerable Handle(AddTodo command)
        {
            yield return new TodoAdded(command.Id, command.Title);
        }

        public IEnumerable Handle(RenameTodo command)
        {
            if (isRemoved) throw new TodoRemovedException();

            yield return new TodoRenamed(command.Id, command.NewTitle);
        }

        public IEnumerable Handle(CompleteTodo command)
        {
            if (isRemoved) throw new TodoRemovedException();

            yield return new TodoCompleted(command.Id);
        }

        public IEnumerable Handle(IncompleteTodo command)
        {
            if (isRemoved) throw new TodoRemovedException();

            yield return new TodoIncompleted(command.Id);
        }

        public IEnumerable Handle(RemoveTodo command)
        {
            yield return new TodoRemoved(command.Id);
        }

        public void Apply(TodoAdded e)
        {
        }

        public void Apply(TodoRemoved e)
        {
            isRemoved = true;
        }

        public void Apply(TodoRenamed e)
        {
        }

        public void Apply(TodoCompleted e)
        {
        }
    }
}
