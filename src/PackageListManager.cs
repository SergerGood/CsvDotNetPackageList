using System.Text;
using CsvDotNetPackageList.Configuration;
using CsvDotNetPackageList.Models;
using static SimpleExec.Command;

namespace CsvDotNetPackageList;

public sealed class PackageListManager
{
    public async IAsyncEnumerable<Package> ProcessAsync(DotNetListSettings settings)
    {
        await foreach (var stdout in GetProcessesStdoutAsync(settings))
        {
            var deserializedObject = Serializer.Deserialize<JsonObject>(stdout);

            foreach (var package in GetPackages(deserializedObject))
                yield return package;
        }
    }

    private static IEnumerable<Package> GetPackages(JsonObject deserializedObject)
    {
        return deserializedObject.Projects.Where(x => x.Frameworks is not null)
            .SelectMany(project => project.Frameworks!
                .SelectMany(framework =>
                {
                    if (framework.TransitivePackages is null)
                        return framework.TopLevelPackages.Select(x => new Package(x.Id, x.ResolvedVersion));

                    return framework.TransitivePackages.Select(x => new Package(x.Id, x.ResolvedVersion))
                        .Concat(framework.TopLevelPackages.Select(x => new Package(x.Id, x.ResolvedVersion)));
                }));
    }


    private static async IAsyncEnumerable<string> GetProcessesStdoutAsync(DotNetListSettings settings)
    {
        foreach (var source in settings.Sources)
        {
            if (string.IsNullOrEmpty(source)) continue;

            yield return await RunProcessAndGetStdout(settings, source);
        }
    }

    private static async Task<string> RunProcessAndGetStdout(DotNetListSettings settings, string source)
    {
        var frameworkArgument = settings.Framework is null
            ? string.Empty
            : $"--framework {settings.Framework}";

        var arguments = $"list {source} package --include-transitive --format json {frameworkArgument}";
        var workingDirectory = settings.WorkingDirectory ?? Directory.GetCurrentDirectory();

        var (stdout, _) = await ReadAsync("dotnet", arguments, workingDirectory, encoding: Encoding.UTF8);
        return stdout;
    }
}
