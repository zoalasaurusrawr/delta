namespace Delta.Core.Models;
internal class DifferenceModel
{
    public List<DifferenceEntry> Differences { get; } = new List<DifferenceEntry>();
}

internal class DifferenceEntry
{
    public DifferenceEntry(string name, object? left, object? right, DifferenceType differenceType)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Left = left;
        Right = right;
        DifferenceType = differenceType;
    }

    public string Name { get; set; } = string.Empty;
    public object? Left { get; set; }
    public object? Right { get; set; }
    public DifferenceType DifferenceType { get; set; } = DifferenceType.None;
    public string DifferentTypeName => Enum.GetName(typeof(DifferenceType), DifferenceType);
}

internal enum DifferenceType
{
    None,
    Added,
    Removed,
    Changed
}
