using System;

namespace Todo.Events
{
    public class TodoIncompleted
    {
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoIncompleted)}#{Id}";
        }
    }
}
