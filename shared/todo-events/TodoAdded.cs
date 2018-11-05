using System;

namespace Todo.Events
{
    public class TodoAdded
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}
