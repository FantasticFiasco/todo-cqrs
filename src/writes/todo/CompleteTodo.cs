using System;

namespace Todo
{
    public class CompleteTodo
    {
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(CompleteTodo)}#{Id}";
        }
    }
}
