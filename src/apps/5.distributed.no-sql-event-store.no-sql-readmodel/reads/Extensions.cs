using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.NoSql;

namespace Reads
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection self)
        {
            Schema.Create();

            return self;
        }

        public static IServiceCollection AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            // Read model
            self
                .AddSingleton(_ => BuildConnectionString(configuration))
                .AddSingleton<ITodoList, NoSqlTodoList>();

            return self;
        }

        private static ConnectionString BuildConnectionString(IConfiguration configuration)
        {
            var host = configuration["READ_MODEL_HOST"];
            var username = configuration["READ_MODEL_USER"];
            var password = configuration["READ_MODEL_PASSWORD"];

            return new ConnectionString(host, username, password);
        }
    }
}
