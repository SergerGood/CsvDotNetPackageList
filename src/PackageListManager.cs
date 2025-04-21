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
        foreach (var project in deserializedObject.Projects)
        {
            if (project.Frameworks is null)
            {
                AnsiConsole.MarkupLine($"[yellow]Project was skipped by Framework:[/][bold yellow] {project.Path}[/]");
            }
            else
            {
                return project.Frameworks
                    .SelectMany(framework =>
                    {
                        if (framework.TransitivePackages is null)
                            return framework.TopLevelPackages.Select(x => new Package(x.Id, x.ResolvedVersion));

                        return framework.TransitivePackages.Select(x => new Package(x.Id, x.ResolvedVersion))
                            .Concat(framework.TopLevelPackages.Select(x => new Package(x.Id, x.ResolvedVersion)));
                    });
            }
        }

        return [];
    }


    private async IAsyncEnumerable<string> GetProcessesStdoutAsync(ProgressTask progress)
    {
        var workingDirectory = settings.Value.WorkingDirectory;
        
        foreach (var source in settings.Value.Sources)
        {
            progress.Increment(1);

            var filePath = source;
            if (!string.IsNullOrEmpty(workingDirectory) && !source.Contains(workingDirectory))
            {
                filePath = Path.Combine(workingDirectory, source);
            }

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                AnsiConsole.MarkupLine($"[yellow]File was not found:[/][bold yellow] {filePath}[/]");
                continue;
            }

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
