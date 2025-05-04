namespace CsvDotNetPackageList.Configuration;

public sealed class DotNetListSettings
{
    public string? Framework { get; init; }
    public string? WorkingDirectory { get; init; }
    public IReadOnlyList<string>? Sources { get; init; }
}