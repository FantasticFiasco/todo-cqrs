using System;

namespace Todo.Events
{
    public class ItemAdded
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}
