using Castle.DynamicProxy;

namespace caching_with_attributes_api.Caching
{
    public static class ServicesExtensions
    {
        public static void AddProxiedScoped<TInterface, TImplementation>
            (this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddScoped<TImplementation>();
            services.AddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider
                    .GetRequiredService<ProxyGenerator>();

                var actual = serviceProvider
                    .GetRequiredService<TImplementation>();

                var interceptors = serviceProvider
                    .GetServices<IInterceptor>().ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTarget(
                    typeof(TInterface), actual, interceptors);
            });
        }
    }
}