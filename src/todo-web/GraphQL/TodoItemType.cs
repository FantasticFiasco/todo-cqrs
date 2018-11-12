using System;
using GraphQL.Types;
using Todo.ReadModel;

namespace Todo.Web.GraphQL
{
    public class TodoItemType : ObjectGraphType<TodoItem>
    {
        public TodoItemType()
        {
            Name = "Todo";

            Field(self => self.Id, type: typeof(IdGraphType)).Description("The id of the todo.");
            Field(self => self.Title).Description("The title of the todo.");
            Field(self => self.IsCompleted).Description("Whether todo is completed.");
        }
    }
}
