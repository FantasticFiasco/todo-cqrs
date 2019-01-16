using System;

namespace Todo
{
    public class TodoAlreadyExistsException : Exception
    {
        public TodoAlreadyExistsException(Guid id)
            : base($"Todo with id {id} already exists")
        {
        }
    }
}
