using System.Text;
using CsvDotNetPackageList.Configuration;
using CsvDotNetPackageList.Models;
using Microsoft.Extensions.Options;
using Spectre.Console;
using static SimpleExec.Command;

namespace CsvDotNetPackageList;

public sealed class PackageListManager(
    IOptions<DotNetListSettings> settings,
    ProgressRunner progressRunner)
{
    public async IAsyncEnumerable<Package> ProcessAsync()
    {
        var (progress, task) = progressRunner.Start();

        await foreach (var stdout in GetProcessesStdoutAsync(progress))
        {
            var deserializedObject = Serializer.Deserialize<JsonObject>(stdout);
            
            foreach (var package in GetPackages(deserializedObject))
                yield return package;
        }

        progress.StopTask();
        await task;
    }

    private static IEnumerable<Package> GetPackages(JsonObject deserializedObject)
    {
        return deserializedObject.Projects.Where(x => x.Frameworks is not null)
            .SelectMany(project => project.Frameworks)
                .SelectMany(framework =>
                {
                    if (framework.TransitivePackages is null)
                        return framework.TopLevelPackages.Select(x => new Package(x.Id, x.ResolvedVersion));

                    return framework.TransitivePackages.Select(x => new Package(x.Id, x.ResolvedVersion))
                        .Concat(framework.TopLevelPackages.Select(x => new Package(x.Id, x.ResolvedVersion)));
                });
    }


    private async IAsyncEnumerable<string> GetProcessesStdoutAsync(ProgressTask progress)
    {
        foreach (var source in settings.Value.Sources)
        {
            progress.Increment(1);

            if (string.IsNullOrEmpty(source) || !File.Exists(source))
                continue;

            yield return await RunProcessAndGetStdout(source);
        }
    }

    private async Task<string> RunProcessAndGetStdout(string source)
    {
        var frameworkArgument = settings.Value.Framework is null
            ? string.Empty
            : $"--framework {settings.Value.Framework}";

        var arguments = $"list {source} package --include-transitive --format json {frameworkArgument}";
        var workingDirectory = settings.Value.WorkingDirectory ?? Directory.GetCurrentDirectory();

        var (stdout, _) = await ReadAsync("dotnet", arguments, workingDirectory, encoding: Encoding.UTF8);
        return stdout;
    }
}
