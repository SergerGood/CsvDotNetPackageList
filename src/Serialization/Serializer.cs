namespace CsvDotNetPackageList.Serialization;

public static class Serializer
{
    private static readonly JsonSerializerOptions readOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, readOptions);
}