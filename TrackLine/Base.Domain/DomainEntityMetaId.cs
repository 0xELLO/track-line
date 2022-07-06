using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class DomainEntityMetaId : DomainEntityMetaId<Guid>, IDomainEntityId
{
}

public abstract class DomainEntityMetaId<TKey> : DomainEntityId<TKey> , IDomainEntityMeta
    where TKey : IEquatable<TKey>
{
    [MaxLength(32)]
    public string? CratedBy { get; set; } 
    public DateTime CratedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(32)]
    public string? UpdatedBy { get; set; } 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}