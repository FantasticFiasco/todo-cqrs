using System;

namespace Todo.Commands
{
    public class Rename
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }
    }
}
