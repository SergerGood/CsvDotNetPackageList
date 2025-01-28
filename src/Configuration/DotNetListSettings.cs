namespace CsvDotNetPackageList.Configuration;

public record DotNetListSettings(
    string? Framework,
    string? WorkingDirectory,
    IReadOnlyList<string> Sources);
