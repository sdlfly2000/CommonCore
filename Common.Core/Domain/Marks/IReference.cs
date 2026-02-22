namespace Common.Core.Domain.Marks;

public interface IReference
{
    string Code { get; set; }

    string CacheFieldName { get; set; }

    string CacheCode { get; }
}
