using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;

namespace caching_with_attributes_api.Caching
{
    public class CacheInterceptor : IInterceptor
    {
        private IMemoryCache _memoryCache;
        public CacheInterceptor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private static string GenerateCacheKey(string name,
            object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                return name;
            return name + "--" +
                string.Join("--", arguments.Select(a =>
                    a == null ? "**NULL**" : a.ToString()).ToArray());
        }

        public void Intercept(IInvocation invocation)
        {
            var cacheAttribute = invocation.MethodInvocationTarget
                .GetCustomAttributes(typeof(CacheAttribute), false)
                .FirstOrDefault() as CacheAttribute;

            if (cacheAttribute != null)
            {
                var cacheKey = GenerateCacheKey(invocation.Method.Name,
                    invocation.Arguments);
                if (_memoryCache.TryGetValue(cacheKey, out var value))
                {
                    invocation.ReturnValue = value;
                }
                else
                {
                    invocation.Proceed();
                    TimeSpan cacheDuration = TimeSpan.FromSeconds(cacheAttribute.Seconds);
                    _memoryCache.Set(cacheKey, invocation.ReturnValue,
                        new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow =
                            new System.TimeSpan(hours: cacheDuration.Hours, minutes: cacheDuration.Minutes,
                                seconds: cacheDuration.Seconds)
                        });
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
