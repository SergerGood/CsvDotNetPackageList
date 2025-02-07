using CsvDotNetPackageList;
using CsvDotNetPackageList.Configuration;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<ProgressRunner>()
    .AddSingleton<PackageListManager>()
    .AddSingleton<CsvGenerator>()
    .Configure()
    .BuildServiceProvider();

var csvGenerator = serviceProvider.GetRequiredService<CsvGenerator>();
await csvGenerator.RunAsync();
