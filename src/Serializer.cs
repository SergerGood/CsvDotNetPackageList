using System.Text.Json;

namespace CsvDotNetPackageList;

public static class Serializer
{
    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, ReadOptions);
}