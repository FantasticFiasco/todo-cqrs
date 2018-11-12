using System;

namespace Todo.ReadModel
{
    public class TodoItem : IEquatable<TodoItem>
    {
        public TodoItem(string id, string title, bool isCompleted)
        {
            Id = id;
            Title = title;
            IsCompleted = isCompleted;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsCompleted { get; set; }

        public bool Equals(TodoItem other)
        {
            return Id == other?.Id
                && Title == other?.Title
                && IsCompleted == other?.IsCompleted;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TodoItem);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
