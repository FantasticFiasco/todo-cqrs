using System;

namespace Todo.Events
{
    public class TodoRenamed
    {
        public TodoRenamed()
        {
        }

        public TodoRenamed(Guid id, string newTitle)
        {
            Id = id;
            NewTitle = newTitle;
        }

        public Guid Id { get; set; }

        public string NewTitle { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoRenamed)}#{Id} NewTitle: {NewTitle}";
        }
    }
}
