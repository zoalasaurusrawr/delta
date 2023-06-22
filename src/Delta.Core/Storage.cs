using Spectre.Console;
using System.Text.Json;

namespace Delta.Core;

/// <summary>
/// A simple storage wrapped to ease saving, loading, exists checks against appdata
/// </summary>
public static class Storage
{
    private static string BasePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string DeltaAppFolderName = ".delta-cli";
    public static bool Exists(string filename)
    {
        EnsureDeltaFolderExists();
        filename = Path.Combine(BasePath, DeltaAppFolderName, filename);
        return File.Exists(filename);
    }

    public static void Save(string filename, string content)
    {
        EnsureDeltaFolderExists();
        filename = Path.Combine(BasePath, DeltaAppFolderName, filename);
        AnsiConsole.WriteLine($"Saved scan to {filename}");
        File.WriteAllText(filename, content);
    }

    public static T? Load<T>(string filename)
    {
        EnsureDeltaFolderExists();
        filename = Path.Combine(BasePath, DeltaAppFolderName, filename);

        if (!Exists(filename))
        {
            return default(T);
        }

        var json = File.ReadAllText(filename);
        return JsonSerializer.Deserialize<T>(json);
    }

    private static void EnsureDeltaFolderExists()
    {
        var path = Path.Combine(BasePath, DeltaAppFolderName);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}
