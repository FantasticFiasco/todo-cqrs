using System;
using GraphQL.Types;
using Todo.ReadModel;

namespace Todo.Web.GraphQL
{
    public class TodoQuery : ObjectGraphType<object>
    {
        public TodoQuery(ITodoList todoList)
        {
            Name = "Query";

            Field<ListGraphType<TodoItemType>>(
                "todos",
                resolve: _ => todoList.GetAll());

            Field<TodoItemType>(
                "todo",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "Id of the todo"
                }),
                resolve: context => todoList.Get(context.GetArgument<Guid>("id")));
        }
    }
}
