using Delta.Core.Analysis;
using Delta.Core.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text;

namespace Delta.Core.Commands;

internal sealed class ScanCommand : Command<ScanCommand.Settings>
{
    internal const string ScanTypeBaseline = "baseline";
    internal const string ScanTypeSnapshot = "snapshot";
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-t|--type")]
        public string? ScanType { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.WriteLine($"Starting scan of type: {settings.ScanType}");

        AnsiConsole.Status()
        .Spinner(Spinner.Known.Star)
        .Start("Scanning...", ctx => {
            var analyzer = new RegistryAnalyzer();
            var results = analyzer.Analyze();
            var filename = $"registry-scan-{DateTime.UtcNow.ToString("dd-MM-yyyy")}.json";

            if (settings.ScanType.Equals(ScanTypeBaseline, StringComparison.InvariantCultureIgnoreCase))
            {
                filename = Constants.BaselineFilename;
            }
            else if (settings.ScanType.Equals(ScanTypeSnapshot, StringComparison.InvariantCultureIgnoreCase))
            {
                filename = $"{Constants.FilePrefix}-{DateTime.UtcNow.ToString("dd-MM-yyyy")}.json";

                if (Storage.Exists(Constants.BaselineFilename))
                {
                    var builder = new StringBuilder();
                    var differenceAnalyzer = new DifferenceAnalyzer();

                    var baseline = Storage.Load<RegistryModel>(Constants.BaselineFilename);
                    AnsiConsole.WriteLine($"Comparing snapshot to baseline...");

                    var diff = differenceAnalyzer.Compare(baseline, results);
                }
            }

            var resultsJson = JsonSerializer.Serialize(results);
            Storage.Save(filename, resultsJson);
        });

        
        return 0;
    }
}
