using System;

namespace Todo.Events
{
    public class Renamed
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }
    }
}
