namespace CsvDotNetPackageList;

public sealed class CsvGenerator(PackageListManager packageListManager)
{
    public async Task RunAsync()
    {
        var packages = packageListManager.ProcessAsync();

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
