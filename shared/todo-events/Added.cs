using System;

namespace Todo.Events
{
    public class Added
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}
