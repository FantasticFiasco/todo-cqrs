using System;

namespace Todo.Commands
{
    public class CompleteTodo
    {
        public CompleteTodo(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
