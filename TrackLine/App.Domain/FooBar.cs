using Base.Domain;

namespace App.Domain;

public class FooBar : DomainEntityMetaId
{
    public string Name { get; set; } = default!;
}