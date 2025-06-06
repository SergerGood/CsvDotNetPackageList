namespace CsvDotNetPackageList.Services;

public sealed class ProgressRunner(IOptions<DotNetListSettings> settings)
{
    private readonly ProgressColumn[] _progressColumns =
    [
        new TaskDescriptionColumn(),
        new ProgressBarColumn(),
        new PercentageColumn(),
        new SpinnerColumn(),
        new ElapsedTimeColumn()
    ];

    public (ProgressTask progress, Task task) Start()
    {
        ProgressTask progress = null!;

        var task = AnsiConsole.Progress()
            .Columns(_progressColumns)
            .StartAsync(async context =>
            {
                progress = context.AddTask("[blue]Processing[/]", true, settings.Value.Sources?.Count ?? 1);

                while (!context.IsFinished)
                {
                    await Task.Delay(100);
                }
            });

        return (progress, task);
    }
}