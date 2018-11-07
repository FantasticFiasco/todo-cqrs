using System;

namespace Todo
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
