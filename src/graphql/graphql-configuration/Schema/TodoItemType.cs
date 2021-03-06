using GraphQL.Types;
using ReadModel;

namespace GraphQL.Configuration.Schema
{
    public class TodoItemType : ObjectGraphType<TodoItem>
    {
        public TodoItemType()
        {
            Name = "TodoItem";

            Field(nameof(TodoItem.Id), self => self.Id.ToString()).Description("The id of the todo.");
            Field(self => self.Title).Description("The title of the todo.");
            Field(self => self.IsCompleted).Description("Whether todo is completed.");
        }
    }
}
