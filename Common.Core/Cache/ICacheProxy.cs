namespace Common.Core.DependencyInjection
{
    public interface ICacheProxy<TResponse> where TResponse : class
    {
        object SetCache(string key, TResponse value);

        TResponse GetCache(string key);
    }
}
