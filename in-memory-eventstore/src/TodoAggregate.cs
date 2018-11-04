using System.Collections;
using Cqrs;
using Todo.Commands;
using Todo.Events;

namespace InMemoryEventstore
{
    public class TodoAggregate : Aggregate,
        IHandleCommand<AddItem>,
        IHandleCommand<RenameItem>,
        IHandleCommand<CompleteItem>,
        IHandleCommand<IncompleteItem>,
        IHandleCommand<RemoveItem>
    {
        public IEnumerable Handle(AddItem command)
        {
            yield return new ItemAdded
            {
                Id = command.Id,
                Title = command.Title
            };
        }

        public IEnumerable Handle(RenameItem command) => throw new System.NotImplementedException();

        public IEnumerable Handle(CompleteItem command) => throw new System.NotImplementedException();

        public IEnumerable Handle(IncompleteItem command) => throw new System.NotImplementedException();

        public IEnumerable Handle(RemoveItem command) => throw new System.NotImplementedException();
    }
}
