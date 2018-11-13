using GraphQL.Types;

namespace Todo.Web.GraphQL
{
    public class IncompleteTodoType : ObjectGraphType<IncompleteTodo>
    {
        public IncompleteTodoType()
        {
            Name = "IncompleteTodo";

            Field(self => self.Id, type: typeof(IdGraphType)).Description("The id of the todo.");
        }
    }
}
