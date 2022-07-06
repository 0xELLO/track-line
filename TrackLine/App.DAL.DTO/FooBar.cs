using Base.Domain;

namespace App.DAL.DTO;

public class FooBar : DomainEntityId
{
    public string Name { get; set; } = default!;
}