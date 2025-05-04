namespace CsvDotNetPackageList.Services;

public sealed class CsvGenerator(PackageListManager packageListManager)
{
    private const string OutputFileName = "packages.csv";

    public async Task RunAsync()
    {
        var packages = packageListManager.GetPackagesAsync();

        var uniquePackages = packages.Distinct()
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Version);

        await using var writer = new StreamWriter(OutputFileName, false);

        await foreach (var uniquePackage in uniquePackages)
        {
            var packageCsvLine = string.Join(';', uniquePackage.Id, uniquePackage.Version);
            await writer.WriteLineAsync(packageCsvLine);
        }

        var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), OutputFileName);
        AnsiConsole.MarkupLine($"[green]CSV file generated:[/][bold green] {outputFilePath}[/]");
    }
}