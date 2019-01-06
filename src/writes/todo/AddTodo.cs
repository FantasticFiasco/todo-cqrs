using System;

namespace Todo
{
    public class AddTodo
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return $"{nameof(AddTodo)}#{Id} Title: {Title}";
        }
    }
}
