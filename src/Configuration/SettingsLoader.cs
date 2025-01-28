using Microsoft.Extensions.Configuration;

namespace CsvDotNetPackageList.Configuration;

public sealed class SettingsLoader
{
    private readonly Lazy<IConfigurationRoot> _configuration = new(Load, LazyThreadSafetyMode.ExecutionAndPublication);

    public T Get<T>()
    {
        var settingsKey = typeof(T).Name;

        return _configuration.Value.GetRequiredSection(settingsKey).Get<T>()
               ?? throw new InvalidOperationException($"Configuration '{settingsKey}' is not set");
    }

    private static IConfigurationRoot Load() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();
}
