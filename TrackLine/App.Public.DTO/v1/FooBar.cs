using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;
[DisplayName("FooBar")]
public class FooBar : DomainEntityId
{
    [Required]
    public string Name { get; set; } = default!;
}