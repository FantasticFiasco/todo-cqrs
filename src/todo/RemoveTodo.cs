using System;

namespace Todo
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
