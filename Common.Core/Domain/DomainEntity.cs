using Common.Core.Domain.Marks;

namespace Common.Core.Domain;

public interface DomainEntity<out TReference> where TReference : IReference
{
    public TReference Id { get; }
}
