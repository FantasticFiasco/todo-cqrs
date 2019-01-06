using System;

namespace Todo.Events
{
    public class TodoCompleted
    {
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoCompleted)}#{Id}";
        }
    }
}
