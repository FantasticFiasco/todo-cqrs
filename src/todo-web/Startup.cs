using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Todo.Web.GraphQL;

namespace Todo.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCqrs();

            // GraphQL
            services.AddSingleton<IDependencyResolver>(serviceProvider => new FuncDependencyResolver(serviceProvider.GetRequiredService));
            services.AddSingleton<ISchema, TodoSchema>();
            services.AddGraphQL();

            // Queries
            services.AddSingleton<TodoQuery>();
            services.AddSingleton<TodoItemType>();
            services.AddSingleton<IdGraphType>();

            // Mutations
            services.AddSingleton<TodoMutation>();
            services.AddSingleton<AddTodoType>();
            services.AddSingleton<CompleteTodoType>();
            services.AddSingleton<IncompleteTodoType>();
            services.AddSingleton<RemoveTodoType>();
            services.AddSingleton<RenameTodoType>();
            services.AddSingleton<GuidGraphType>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL<ISchema>();

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground"
            });
        }
    }
}
