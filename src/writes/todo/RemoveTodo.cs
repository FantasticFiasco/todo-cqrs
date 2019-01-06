using System;

namespace Todo
{
    public class RemoveTodo
    {
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(RemoveTodo)}#{Id}";
        }
    }
}
