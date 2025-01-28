namespace CsvDotNetPackageList.Models;

public record JsonObject(int Version, string Parameters, List<Project> Projects);

public record Project(string Path, List<Framework>? Frameworks);

public record Framework(
    string framework,
    List<TopLevelPackage> TopLevelPackages,
    List<TransitivePackage>? TransitivePackages);

public record TransitivePackage(string Id, string ResolvedVersion);

public record TopLevelPackage(string Id, string RequestedVersion, string ResolvedVersion, string AutoReferenced);
