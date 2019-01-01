using System;

namespace Todo.Events
{
    public class TodoIncompleted
    {
        public TodoIncompleted()
        {
        }

        public TodoIncompleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoIncompleted)}#{Id}";
        }
    }
}
