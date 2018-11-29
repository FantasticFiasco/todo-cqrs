﻿using GraphQL.Types;

namespace Todo.Web.GraphQL.Schema
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