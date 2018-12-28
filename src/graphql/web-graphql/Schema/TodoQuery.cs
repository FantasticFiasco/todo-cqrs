using System;
using GraphQL.Types;
using Todo.ReadModel;

namespace GraphQL.Schema
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
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "Id of the todo" }
                ),
                resolve: context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    return todoList.Get(id);
                });
        }
    }
}
