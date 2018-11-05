using System;

namespace Todo.Commands
{
    public class RemoveTodo
    {
        public RemoveTodo(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
