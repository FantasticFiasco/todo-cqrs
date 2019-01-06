using System;

namespace Todo.Events
{
    public class TodoRemoved
    {
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoRemoved)}#{Id}";
        }
    }
}
