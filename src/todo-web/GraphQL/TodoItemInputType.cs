using GraphQL.Types;
using Todo.ReadModel;

namespace Todo.Web.GraphQL
{
    public class TodoItemInputType : InputObjectGraphType<TodoItem>
    {
        public TodoItemInputType()
        {
            Name = "Todo";

            Field(self => self.Title);
        }
    }
}
