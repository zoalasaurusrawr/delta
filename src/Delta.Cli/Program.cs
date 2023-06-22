using Delta.Core.Commands;
using Spectre.Console.Cli;

if (File.Exists(@"cli.txt"))
{
    var cliText = File.ReadAllLines(@"cli.txt");
    foreach (var line in cliText)
    {
        Console.WriteLine(line);
    }
    Console.WriteLine();
}

var app = new CommandApp<ScanCommand>();
app.Run(args);