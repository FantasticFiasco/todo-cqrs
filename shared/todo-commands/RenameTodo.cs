using System;

namespace Todo.Commands
{
    public class RenameTodo
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }
    }
}
