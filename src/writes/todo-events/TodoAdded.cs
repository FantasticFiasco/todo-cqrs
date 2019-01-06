using System;

namespace Todo.Events
{
    public class TodoAdded
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoAdded)}#{Id} Title: {Title}";
        }
    }
}
