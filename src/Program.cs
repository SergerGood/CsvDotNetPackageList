using CsvDotNetPackageList;
using CsvDotNetPackageList.Configuration;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<ProgressRunner>()
    .AddSingleton<PackageListManager>()
    .AddSingleton<CsvGenerator>()
    .ConfigureSettings()
    .BuildServiceProvider();

var csvGenerator = serviceProvider.GetRequiredService<CsvGenerator>();
await csvGenerator.RunAsync();
