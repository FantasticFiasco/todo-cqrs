using System;

namespace Todo
{
    public class RenameTodo
    {
        public Guid Id { get; set; }

        public string NewTitle { get; set; }

        public override string ToString()
        {
            return $"{nameof(RenameTodo)}#{Id} Title: {NewTitle}";
        }
    }
}
