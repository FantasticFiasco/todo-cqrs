using GraphQL.Types;

namespace Todo.Web.GraphQL
{
    public class RenameTodoType : ObjectGraphType<RenameTodo>
    {
        public RenameTodoType()
        {
            Name = "RenameTodo";

            Field(self => self.Id, type: typeof(IdGraphType)).Description("The id of the todo.");
            Field(self => self.NewTitle).Description("The new title of the todo.");
        }
    }
}
