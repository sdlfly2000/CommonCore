namespace Common.Core.AOP.Cache
{
    public interface IReference
    {
        string Code { get; set; }

        string CacheFieldName { get; set; }

        string CacheCode { get; }
    }
}
