using Base.Domain;

namespace App.BLL.DTO;

public class FooBar : DomainEntityId
{
    public string Name { get; set; } = default!;
}