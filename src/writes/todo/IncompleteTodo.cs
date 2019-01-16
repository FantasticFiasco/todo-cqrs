using System;

namespace Todo
{
    public class IncompleteTodo
    {
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(IncompleteTodo)}#{Id}";
        }
    }
}
