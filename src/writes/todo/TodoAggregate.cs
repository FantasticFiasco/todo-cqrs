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
        IApplyEvent<TodoCompleted>,
        IApplyEvent<TodoIncompleted>
    {
        private bool exists;

        public IEnumerable Handle(AddTodo command)
        {
            if (exists) throw new TodoAlreadyExistsException(command.Id);

            yield return new TodoAdded
            {
                Id = command.Id,
                Title = command.Title
            };
        }

        public IEnumerable Handle(RenameTodo command)
        {
            if (!exists) throw new TodoDoesNotExistException(command.Id);

            yield return new TodoRenamed
            {
                Id = command.Id,
                NewTitle = command.NewTitle
            };
        }

        public IEnumerable Handle(CompleteTodo command)
        {
            if (!exists) throw new TodoDoesNotExistException(command.Id);

            yield return new TodoCompleted
            {
                Id = command.Id
            };
        }

        public IEnumerable Handle(IncompleteTodo command)
        {
            if (!exists) throw new TodoDoesNotExistException(command.Id);

            yield return new TodoIncompleted
            {
                Id = command.Id
            };
        }

        public IEnumerable Handle(RemoveTodo command)
        {
            if (!exists) throw new TodoDoesNotExistException(command.Id);

            yield return new TodoRemoved
            {
                Id = command.Id
            };
        }

        public void Apply(TodoAdded e)
        {
            exists = true;
        }

        public void Apply(TodoRemoved e)
        {
            exists = false;
        }

        public void Apply(TodoRenamed e)
        {
        }

        public void Apply(TodoCompleted e)
        {
        }

        public void Apply(TodoIncompleted e)
        {
        }
    }
}
