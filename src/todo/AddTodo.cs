using System;

namespace Todo
{
    public class AddTodo
    {
        public AddTodo(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public Guid Id { get; }

        public string Title { get; }
    }
}
