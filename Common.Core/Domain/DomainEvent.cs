using System;

namespace Common.Core.Domain;

public abstract class DomainEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
