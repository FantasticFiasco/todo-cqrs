using System;

namespace Todo.Commands
{
    public class IncompleteTodo

    {
        public IncompleteTodo(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
