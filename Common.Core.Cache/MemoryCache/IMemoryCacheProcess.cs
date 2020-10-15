namespace Common.Core.Cache.MemoryCache
{
    public interface IMemoryCacheProcess
    {
        object Get(string Code);

        object Set(string Code, object value);
    }
}
