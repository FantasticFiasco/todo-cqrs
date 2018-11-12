using System;
using Cqrs;
using GraphQL.Types;
using Todo.ReadModel;

namespace Todo.Web.GraphQL
{
    public class TodoMutation : ObjectGraphType
    {
        public TodoMutation(MessageDispatcher messageDispatcher)
        {
            Name = "Mutation";

            Field<TodoItemType>(
                "createTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" }
                ),
                resolve: context =>
                {
                    var id = Guid.NewGuid();
                    var title = context.GetArgument<string>("title");

                    messageDispatcher.SendCommand(new AddTodo(id, title));

                    return new TodoItem(id, title, false);
                });
        }
    }
}
