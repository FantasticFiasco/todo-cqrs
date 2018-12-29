using System;

namespace Todo.Events
{
    public class TodoCompleted
    {
        public TodoCompleted()
        {
        }

        public TodoCompleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}