using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CsvDotNetPackageList.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSettings(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        var configurationSection = configuration.GetSection(nameof(DotNetListSettings));
        services.Configure<DotNetListSettings>(configurationSection);

        return services;
    }
}
