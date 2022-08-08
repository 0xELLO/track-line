using System.ComponentModel;

namespace App.Public.DTO.v1.Helpers;

[DisplayName("RestApiErrorResponse")]
public class RestApiErrorResponse
{
    public string Type { get; set; } = default!;
    public int Status { get; set; }
    public string TraceId { get; set; } = default!;
    public Dictionary<string, string> Errors { get; set; } = new();
}