using System;
using GraphQL.Types;
using ReadModel;

namespace Web.GraphQL.Schema
{
    public class Query : ObjectGraphType<object>
    {
        public Query(ITodoList todoList)
        {
            Name = "Query";

            Field<ListGraphType<TodoItemType>>(
                "todos",
                resolve: _ => todoList.GetAllAsync());

            Field<TodoItemType>(
                "todo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "Id of the todo" }
                ),
                resolve: context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    return todoList.GetAsync(id);
                });
        }
    }
}
