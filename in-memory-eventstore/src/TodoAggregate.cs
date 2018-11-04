using System.Collections;
using Cqrs;
using Todo.Commands;
using Todo.Events;

namespace InMemoryEventstore
{
    public class TodoAggregate : Aggregate,
        IHandleCommand<Add>,
        IHandleCommand<Rename>,
        IHandleCommand<Complete>,
        IHandleCommand<Incomplete>,
        IHandleCommand<Remove>
    {
        public IEnumerable Handle(Add command)
        {
            yield return new Added
            {
                Id = command.Id,
                Title = command.Title
            };
        }

        public IEnumerable Handle(Rename command) => throw new System.NotImplementedException();

        public IEnumerable Handle(Complete command) => throw new System.NotImplementedException();

        public IEnumerable Handle(Incomplete command) => throw new System.NotImplementedException();

        public IEnumerable Handle(Remove command) => throw new System.NotImplementedException();
    }
}
