using System;
using GraphQL.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public static class Extensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            // Write model
            self
                .AddSingleton<IMutation, Mutation>()
                .AddHttpClient<MutationClient>(client =>
                {
                    var host = configuration["WRITES_HOST"];

                    client.BaseAddress = new Uri($"http://{host}");
                });


            // Read model
            self
                .AddSingleton<IQuery, Query>()
                .AddHttpClient<QueryClient>(client =>
                {
                    var host = configuration["READS_HOST"];

                    client.BaseAddress = new Uri($"http://{host}");
                });

            return self;
        }
    }
}
