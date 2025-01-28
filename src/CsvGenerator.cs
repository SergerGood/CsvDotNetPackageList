using CsvDotNetPackageList.Models;

namespace CsvDotNetPackageList;

public sealed class CsvGenerator
{
    public async Task RunAsync(IAsyncEnumerable<Package> packages)
    {
        var uniquePackages = packages.Distinct()
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Version);
        
        await using var writer = new StreamWriter("packages.csv", false);
        
        await foreach (var uniquePackage in uniquePackages)
        {
            var packageCsvLine = string.Join(';', uniquePackage.Id, uniquePackage.Version);
            await writer.WriteLineAsync(packageCsvLine);
        }
    }
}
