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

            services.AddSingleton<IDependencyResolver>(serviceProvider => new FuncDependencyResolver(serviceProvider.GetRequiredService));
            services.AddSingleton<ISchema, TodoSchema>();
            services.AddSingleton<TodoQuery>();
            services.AddSingleton<TodoItemType>();
            services.AddSingleton<IdGraphType>();
            services.AddGraphQL();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL<ISchema>("/graphql");

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground"
            });
        }
    }
}
