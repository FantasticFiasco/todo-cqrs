using System;

namespace Todo.Events
{
    public class TodoRenamed
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }

        public override string ToString()
        {
            return $"{nameof(TodoRenamed)}#{Id} NewTitle: {NewTitle}";
        }
    }
}
