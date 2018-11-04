using System;

namespace Todo.Commands
{
    public class RenameItem
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }
    }
}
