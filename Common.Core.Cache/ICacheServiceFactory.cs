namespace Common.Core.Cache
{
    public interface ICacheServiceFactory
    {
        object Get(string Code);

        object Set(string Code, object value);
    }
}
