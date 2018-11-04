using System;

namespace Todo.Events
{
    public class ItemRenamed
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }
    }
}
