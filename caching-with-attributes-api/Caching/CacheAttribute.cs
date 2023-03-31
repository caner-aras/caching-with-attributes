namespace caching_with_attributes_api.Caching;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CacheAttribute : Attribute
{
    public int Seconds { get; set; } = 30;
}