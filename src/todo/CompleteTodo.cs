using System;

namespace Todo
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
