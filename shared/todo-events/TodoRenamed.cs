using System;

namespace Todo.Events
{
    public class TodoRenamed
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }
    }
}
