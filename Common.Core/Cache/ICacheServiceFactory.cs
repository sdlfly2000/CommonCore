namespace Common.Core.Cache
{
    public interface ICacheServiceFactory
    {
        object Get(string Code);

        T Get<T>(string Code) where T: class;

        object Set(string Code, object value);
    }
}
