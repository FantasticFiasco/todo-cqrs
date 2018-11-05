using System;

namespace Todo.Commands
{
    public class RenameTodo
    {
        public RenameTodo(Guid id, string newTitle)
        {
            Id = id;
            NewTitle = newTitle;
        }

        public Guid Id { get; }

        public string NewTitle { get; }
    }
}
