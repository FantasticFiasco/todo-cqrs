using System;

namespace Todo.Events
{
    public class TodoRemoved
    {
        public TodoRemoved()
        {
        }

        public TodoRemoved(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoRemoved)}#{Id}";
        }
    }
}
