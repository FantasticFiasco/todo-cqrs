using GraphQL.Types;
using Todo;

namespace GraphQL.Schema
{
    public class CompleteTodoType : ObjectGraphType<CompleteTodo>
    {
        public CompleteTodoType()
        {
            Name = "CompleteTodo";

            Field(self => self.Id, type: typeof(IdGraphType)).Description("The id of the todo.");
        }
    }
}
