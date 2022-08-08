using System.Text.Json;

namespace Tests.WebApp.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions JsonSerializerOptions =
        new JsonSerializerOptions(JsonSerializerDefaults.Web);
        
    public static TValue? DeserializeWithWebDefaults<TValue>(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<TValue>(json, JsonSerializerOptions);
    }

    public static string? SerializeWithWebDefaults<TValue>(TValue Object)
    {
        return System.Text.Json.JsonSerializer.Serialize<TValue>(Object, JsonSerializerOptions);
    }
}