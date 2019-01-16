// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(this IServiceCollection self)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            return self
                .AddSingleton<TImplementation>()
                .AddSingleton<TService1>(provider => provider.GetRequiredService<TImplementation>())
                .AddSingleton<TService2>(provider => provider.GetRequiredService<TImplementation>());
        }
    }
}
